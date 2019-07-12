using System;
using TMPro;

namespace UniRx
{
    public static partial class TextMeshProExtension
    {
        public static IObservable<string> OnValueChangedAsObservable(this TMP_InputField inputField) {
            return Observable.CreateWithState<string, TMP_InputField>(
                inputField, (i, observer) => {
                    observer.OnNext(i.text);
                    return i.onValueChanged.AsObservable().Subscribe(observer);
                }
            );
        }
        
        public static IObservable<string> OnEndEditAsObservable(this TMP_InputField inputField) {
            return Observable.CreateWithState<string, TMP_InputField>(
                inputField, (i, observer) => {
                    observer.OnNext(i.text);
                    return i.onEndEdit.AsObservable().Subscribe(observer);
                }
            );
        }
    }
}