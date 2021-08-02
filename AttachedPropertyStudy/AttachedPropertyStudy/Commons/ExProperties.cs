using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AttachedPropertyStudy.Commons
{
    public class ExProperties
    {
        // PasswordBox는 보안을 이유로 Password 프로퍼티에 바인딩 불가능. PasswordBox를 상속받는 컨트롤 또한 만들 수 없다. 
        // 이런 경우에 문제를 해결하기 위해 AttachedProperty를 이용.

        // BindablePassword : 바인딩을 하기 위한 프로퍼티 / IsPasswordBindable : 바인딩을 할지 말지 여부를 정하는 프로퍼티.
        // BindablePassword 프로퍼티 한개만 가지고는 PasswordBox와 연결해서 값을 가져올 수 없다. 값이 변경되면 변경된 값을 넣어야 하는데, 이벤트 연결이 되어있지 않기 때문.
        // 따라서 IsPasswordBindable 프로퍼티의 값이 True가 되면, PasswordChanged 이벤트 핸들러를 추가하고, 그 이벤트 핸들러 내에서 BindablePassword에 Password 값을 변경하는 형태로 이용.

        public static string GetBindablePassword(DependencyObject obj)
        {
            return (string)obj.GetValue(BindablePasswordProperty);
        }

        public static void SetBindablePassword(DependencyObject obj, string value)
        {
            obj.SetValue(BindablePasswordProperty, value);
        }

        public static readonly DependencyProperty BindablePasswordProperty =
            DependencyProperty.RegisterAttached("BindablePassword", typeof(string), typeof(ExProperties), new PropertyMetadata(null));


        public static bool GetIsPasswordBindable(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsPasswordBindableProperty);
        }

        public static void SetIsPasswordBindable(DependencyObject obj, bool value)
        {
            obj.SetValue(IsPasswordBindableProperty, value);
        }

        public static readonly DependencyProperty IsPasswordBindableProperty =
            DependencyProperty.RegisterAttached("IsPasswordBindable", typeof(bool), typeof(ExProperties), new PropertyMetadata(false, IsPasswordBindableChanged));

        private static void IsPasswordBindableChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is PasswordBox passwordBox)) return;
            if ((bool)e.NewValue)
            {
                passwordBox.PasswordChanged += PasswordBox_PasswordChanged;
            }
            else
            {
                passwordBox.PasswordChanged -= PasswordBox_PasswordChanged;
            }
        }

        private static void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = (PasswordBox)sender;
            SetBindablePassword(passwordBox, passwordBox.Password);
        }

        // AttachedProeprty는 단독으로 사용할 수 있어 범용성이 좋지만, XAML Style에서 Trigger를 사용해서 값을 직접 비교하는 것은 불가능.
        // 실제 값(Value)를 저장하고 사용하기 위해서 2개의 메소드로만 구성되어 있기 때문.

        // 2개의 메소드로 구분되어 있는 이유 : 값이 설정된 인스턴스의 CLR 네임스페이스의 일부가 아니기 때문. PasswordBox에 값을 저장하고는 있지만,
        // 저장하고 불러올 때 사용하는 이름을 Passwordbox는 모른다. 따라서 모든 이름을 다 입력해 주어야만 사용할 수 있다.

        // 또한 이벤트 핸들러 추가/제거 시점을 특정하기 어렵다. 이벤트 핸들러를 제거하기 위해서는 PasswordBox의 Unloaded 이벤트 핸들러를 추가로 달아서 처리해야한다.
    }
}
