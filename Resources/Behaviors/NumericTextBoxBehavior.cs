using System.Linq;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace Resources.Behaviors
{
    public class NumericTextBoxBehavior : Behavior<TextBox>
    {
        #region Constants

        /// <summary>
        /// Stores the default regular expression
        /// </summary>
        private const string DoubleValidaterExpression = @"^(\d+)?(\.)?(\d{1,2})?$";

        #endregion

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.TextChanged -= TextBoxTextChanged;
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.TextChanged += TextBoxTextChanged;
        }

        private void TextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox == null)
            {
                return;
            }
            var changeSet = e.Changes.FirstOrDefault();

            if (changeSet == null)
            {
                return;
            }

            var cursorPosition = textBox.SelectionStart;

            if (System.Text.RegularExpressions.Regex.IsMatch(textBox.Text,
                                                             DoubleValidaterExpression))
            {
                return;
            }
            var text = textBox.Text.Substring(0,
                                              changeSet.Offset)
                       + textBox.Text.Substring((changeSet.Offset + changeSet.AddedLength));
            // un-subscribe the text changed event
            textBox.TextChanged -= this.TextBoxTextChanged;

            textBox.Text = text;
            // if cursor position is greater than 0 then set the selection start of the text box
            if (cursorPosition > 0)
            {
                textBox.SelectionStart = cursorPosition < text.Length
                                             ? cursorPosition - 1
                                             : text.Length + 1;
            }

            textBox.TextChanged += this.TextBoxTextChanged;
        }
    }
}
