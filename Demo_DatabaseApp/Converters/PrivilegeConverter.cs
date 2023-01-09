// Demo_DatabaseApp/Demo_DatabaseApp/PrivilegeConverter.cs
// Created by Tiran Spierer
// Created at 03/01/2023
// Class propose:

using DataAccess.DataModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace Demo_DatabaseApp.Converters;

public class PrivilegeConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        string result = string.Empty;

        if (value is ICollection<UserPrivilege> privileges)
        {
            result = string.Join(", ", privileges.Select(p => p.Privilege.ToString()));
        }

        return result;

    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}