﻿using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Todos.Converter
{
    public class BooleanToOpacityConverter : IValueConverter
    {
        public BooleanToOpacityConverter() { }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool isChecked = (bool)value;
            return isChecked? 1.0: 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            double opacity = (double)value;
            return opacity == 1.0;
        }
    }

    public class BooleanToNullableConverter : IValueConverter
    {
        public BooleanToNullableConverter() { }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool?)
            {
                return (bool)value;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is bool)
                return (bool)value;
            return false;
        }
    }
}
