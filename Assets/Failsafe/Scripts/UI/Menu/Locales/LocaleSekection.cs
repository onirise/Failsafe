using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;
using System.Collections;
using System.Linq;

public class LocaleSekection : MonoBehaviour
{
    public TMP_Dropdown dropdownLocale;
    Coroutine LocaleChangeCoroutine;

    void Start() 
    {
        
        dropdownLocale.options.Clear();
        foreach (var locale in LocalizationSettings.AvailableLocales.Locales)    
        {
            string result = locale.name.Substring(locale.name.Length - 3);
            result = result.TrimEnd(result[result.Length - 1]);
            result = result.ToUpper();
            dropdownLocale.options.Add(new TMP_Dropdown.OptionData(result));
        }
        dropdownLocale.value = LocalizationSettings.AvailableLocales.Locales.IndexOf(LocalizationSettings.SelectedLocale);

        dropdownLocale.onValueChanged.AddListener((index) =>
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
        });
    }
    IEnumerator SetLocale(int _localeID)
    {
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[_localeID];
        LocaleChangeCoroutine = null;
    }

    public void ChangeLocale(int localeID)
    {
        if(LocaleChangeCoroutine == null)
            LocaleChangeCoroutine = StartCoroutine(SetLocale(localeID));
    }
    
    public void GetDropdownValue()
    {
        int pickedIndex = dropdownLocale.value;
        ChangeLocale(pickedIndex);
    }
}
