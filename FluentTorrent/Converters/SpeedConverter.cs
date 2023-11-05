using ByteSizeLib;
using Microsoft.UI.Xaml.Data;

namespace FluentTorrent.Converters;
public class SpeedConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is not double bytes)
        {
            return "N/A";
        }

        var byteSize = ByteSize.FromBytes(bytes);

        return $"{byteSize.ToString()}/s";
    }
    public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
}