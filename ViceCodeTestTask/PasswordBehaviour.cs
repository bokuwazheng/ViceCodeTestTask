using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace ViceCodeTestTask
{
    public class PasswordBehavior : Behavior<PasswordBox>
    {
        protected override void OnAttached()
        {
            AssociatedObject.PasswordChanged += OnPasswordChanged;
            base.OnAttached();
        }

        protected override void OnDetaching()
        {
            AssociatedObject.PasswordChanged -= OnPasswordChanged;
            base.OnDetaching();
        }

        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register("Password", 
                typeof(SecureString), 
                typeof(PasswordBehavior), 
                new FrameworkPropertyMetadata(null) { BindsTwoWayByDefault = true });

        public SecureString Password
        {
            get { return (SecureString)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }

        private static void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;
            PasswordBehavior behavior = Interaction.GetBehaviors(passwordBox).OfType<PasswordBehavior>().FirstOrDefault();
            if (behavior != null)
            {
                behavior.Password = passwordBox.SecurePassword;
            }
        }
    }
}
