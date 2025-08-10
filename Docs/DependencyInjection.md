# Внедрение зависимостей и архитектура приложения

- [Внедрение зависимостей и архитектура приложения](#Внедрение-зависимостей-и-архитектура-приложения)
  * [VContainer](#vcontainer)
    + [Использование интерфейсов VContainer вместо MonoBehavior](#Использование-интерфейсов-VContainer-вместо-MonoBehavior)
    + [Использование UniTask вместо Coroutine](#Использование-UniTask-вместо-Coroutine)
  * [Архитектура приложения](#Архитектура-приложения)
    + [RootLifetimeScope](#rootlifetimescope)
    + [GameSceneLifetemeScope](#gamescenelifetemescope)
    + [PlayerLifetimeScope](#playerlifetimescope)

## VContainer
Ссылка на официальную документацию https://vcontainer.hadashikick.jp/
В качестве DI контейнера в проекте используется VContainer

### Использование интерфейсов VContainer вместо MonoBehavior
Большинство скриптов реализующих логику и не используют функционал MonoBehavior. Поэтому лучше писать скрипты на чистых классах и регистрировать их в контейнере. Это упрощает работу со скриптами, зависимости пробрасываются автоматически в конструктор, все ссылки между скриптами видны в IDE,  не нужно искать компоненты по разным префабам. Также это повышает производительность.

Если в скрипте необходимы доступы к некоторым методам MonoBehavior, например скрипт должен выполняться при каждом Update, нужно использовать интерфейсы предоставленные VContainer`ом

| Интерфейс VContainer	|   Аналог в MonoBehaviour|
| ------ | ------ |
| IStartable.Start()    |	MonoBehaviour.Start()|
| IAsyncStartable.StartAsync()  |	MonoBehaviour.Start()|
| IPostStartable.PostStart()    |	После MonoBehaviour.Start()|
| IFixedTickable.FixedTick()    |	MonoBehaviour.FixedUpdate()|
| IPostFixedTickable.PostFixedTick()    |	После MonoBehaviour.FixedUpdate()|
| ITickable.Tick()  |	MonoBehaviour.Update()|
| IPostTickable.PostTick()  |	После MonoBehaviour.Update()|
| ILateTickable.LateTick()  |	MonoBehaviour.LateUpdate()|
| IPostLateTickable.PostLateTick()  |	После MonoBehaviour.LateUpdate()|
Подробнее https://vcontainer.hadashikick.jp/integrations/entrypoint
Классы которые реализуют эти интерфейсы нужно регистрировать как EntryPoint
```csharp
builder.RegisterEntryPoint<FooController>();
```

### Использование UniTask вместо Coroutine
В обычных c# классах нельзя использовать Coroutine, потому что они работают только в MonoBehavior. Вместо них нужно использовать UniTask 

Пример корутины:
```csharp
IEnumerator TestCouroutine()
{
    Debug.Log("Couroutine Start");
    yield return null;
    Debug.Log("Couroutine next frame");
    yield return new WaitForSeconds(5);
    Debug.Log("Couroutine after 5 seconds");
}
```
Точно такой же функционал на UniTask: 
```csharp
async UniTask TestUniTask()
{
    Debug.Log("UniTask Start");
    await UniTask.Yield(); //можно использовать await UniTask.NextFrame();
    Debug.Log("UniTask next frame");
    await UniTask.WaitForSeconds(5); //можно использовать await UniTask.Delay(TimeSpan.FromSeconds(5));
    Debug.Log("UniTask after 5 seconds");
}
```

Подробнее какие еще есть функции у UniTask https://github.com/Cysharp/UniTask?tab=readme-ov-file#getting-started

#### Как запустить
Запускаем корутину:
```csharp
StartCoroutine(TestCouroutine());
```

Запускаем юнитаску:
```csharp
TestUniTask().Forget();
```

#### Как остановить
Рекомендуется к прочтению если не работали с CancellationTokenSource https://learn.microsoft.com/ru-ru/dotnet/api/system.threading.cancellationtokensource

Как остановить корутину:
```csharp
Coroutine _coroutine;
...
_coroutine = StartCoroutine(TestCouroutine());
...
StopCoroutine(_coroutine);
```

Юнитаски прерыаются с помощью CancellationToken. чтобы можно было прервать таску нужно ее сперва переписать. Таска должна принимать токен и нужно указать как именно ее прервать
```csharp
async UniTask TestUniTask(CancellationToken cancellationToken = default) // = default чтобы можно было вызывать таску без токена когда это не нужно
{
    Debug.Log("UniTask Start");
    await UniTask.Yield(cancellationToken);
    Debug.Log("UniTask next frame");
    await UniTask.WaitForSeconds(5, cancellationToken: cancellationToken);
    Debug.Log("UniTask End");
}
```
Пример другой таски которая выполняется каждый фрейм пока не отменится:
```csharp
async UniTask TestUniTask2(CancellationToken cancellationToken = default)
{
    while (!cancellationToken.IsCancellationRequested)
    {
        Debug.Log("UniTask one frame");
        await UniTask.Yield();
    }
}
```

Как остановить юнитаску:
```csharp
CancellationTokenSource _cts;
...
// Перед каждым использованием нужно создать новый экземпляр CancellationTokenSource, при этом задиспозить старый
if (_cts != null)
{
    _cts.Cancel();
    _cts.Dispose();
    _cts = null;
}
_cts = new CancellationTokenSource();
TestUniTask(_cts.Token).Forget();
...
_cts.Cancel();
```

## Архитектура приложения
В игре используется несколько LifetimeScope выстроенных в иерархии. В каждом регистрируются определенные системы.

### RootLifetimeScope
LifetimeScope игры. Запускается в первую очередь. Нужен для регистрации систем которые нужны в течении всей игры от запуска до закрытия приложения.
> Примеры систем: Менеджер сцен, Сохранение, Глобальная статистика и т.д.

### GameSceneLifetemeScope
LifetimeScope игровой сцены. Запускается когда загружается сцена игры. Зависит от RootLifetimeScope. Нужен для регистрации систем на уровне, которые используются несколькими объектами на сцене.
> Примеры систем: Спаунер врагов, Система шума, Тревога, Коммуникация между врагами и т.д.

### PlayerLifetimeScope
LifetimeScope игрового персонажа. Зависит от GameSceneLifetemeScope. Тут регистрируются системы и компоненты игрока.
> Примеры систем: Здоровье и выносливость, системы передвижения, инвентарь и т.д.