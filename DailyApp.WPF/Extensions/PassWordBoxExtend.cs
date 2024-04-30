using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DailyApp.WPF.Extensions
{
    public class PassWordBoxExtend
    {


        public static string GetPwd(DependencyObject obj)
        {
            return (string)obj.GetValue(PwdProperty);
        }

        public static void SetPwd(DependencyObject obj, string value)
        {
            obj.SetValue(PwdProperty, value);
        }

        // Using a DependencyProperty as the backing store for Pwd.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PwdProperty =
            DependencyProperty.RegisterAttached("Pwd", typeof(string), typeof(PassWordBoxExtend), new PropertyMetadata("",OnPwdChanged));

        private static void OnPwdChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PasswordBox pwdBox = d as PasswordBox;
            string newPwd = (string)e.NewValue;
            if (pwdBox != null && pwdBox.Password != newPwd) 
            {
            pwdBox.Password = newPwd;
            }
        }
    }

    public class PasswordBoxBehavior : Behavior<PasswordBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PasswordChanged += OnPasswordChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.PasswordChanged -= OnPasswordChanged;
        }

        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;
            string password = PassWordBoxExtend.GetPwd(passwordBox);

            if (passwordBox !=null && passwordBox.Password != password)
            {
                PassWordBoxExtend.SetPwd(passwordBox, passwordBox.Password);
            }
        }
    }
}