﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SymWin;

namespace SymWin
{
   /// <summary>
   /// Interaction logic for LetterSelector.xaml
   /// </summary>
   public partial class LetterSelector : Window
   {
      public LetterSelector(params Char[] letters)
      {
         if (letters == null || letters.Length == 0) throw new ArgumentException("Missing letters");

         InitializeComponent();

         var letterTemplate = Utils.CloneWPFObject(this.LetterPanel.Children.Cast<TextBox>().First());

         // Remove sample children.
         this.LetterPanel.Children.Clear();

         var width = 0.0;

         // Add letters in order of appearance.
         foreach (var letter in letters)
         {
            var newLetter = Utils.CloneWPFObject(letterTemplate);
            newLetter.Text = letter.ToString();

            this.LetterPanel.Children.Add(newLetter);

            width += newLetter.Width;
         }

         // Restrict window size to panel width.
         this.Width = width;
         this.Height = letterTemplate.Height;

         this.Loaded += (_, __) => SelectNext();
      }

      public Char SelectedLetter
      {
         get
         {
            return FindVisualChildren<TextBox>(this).ElementAt(_mActiveIndex).Text[0];
         }
      }

      private Int32 _mActiveIndex = -1;

      public void SelectNext()
      {
         var letters = FindVisualChildren<TextBox>(this);
         var count = letters.Count();

         _mActiveIndex = (_mActiveIndex + 1) % count;

         letters.ElementAt(_mActiveIndex).Focus();
      }

      public void SelectPrevious()
      {
         var letters = FindVisualChildren<TextBox>(this);
         var count = letters.Count();

         _mActiveIndex = (count + _mActiveIndex - 1) % count;

         letters.ElementAt(_mActiveIndex).Focus();
      }

      private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
      {
      }

      public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
      {
         if (depObj != null)
         {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
               DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
               if (child != null && child is T)
               {
                  yield return (T)child;
               }

               foreach (T childOfChild in FindVisualChildren<T>(child))
               {
                  yield return childOfChild;
               }
            }
         }
      }

      private void OnWindowDeactivated(object sender, EventArgs e)
      {
         this.Visibility = System.Windows.Visibility.Hidden;
      }
   }
}