using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using Microsoft.UI;
using Microsoft.UI.Xaml.Media;

namespace FluentTorrent.Views.Controls;

public sealed partial class SpeedIndicator : UserControl
{
    public SpeedIndicator() => InitializeComponent();

    public static readonly DependencyProperty SpeedProperty =
        DependencyProperty.Register("Speed", typeof(string), typeof(SpeedIndicator), new PropertyMetadata("50MB/s"));

    public string Speed
    {
        get => (string)GetValue(SpeedProperty);
        set => SetValue(SpeedProperty, value);
    }

    public static readonly DependencyProperty SizeProperty =
        DependencyProperty.Register("Size", typeof(string), typeof(SpeedIndicator), new PropertyMetadata("36.18GB"));

    public string Size
    {
        get => (string)GetValue(SizeProperty);
        set => SetValue(SizeProperty, value);
    }

    public static readonly DependencyProperty IconColorProperty =
        DependencyProperty.Register("IconColor", typeof(Brush), typeof(SpeedIndicator), new PropertyMetadata(Colors.LightBlue));

    public Brush IconColor
    {
        get => (Brush)GetValue(IconColorProperty);
        set => SetValue(IconColorProperty, value);
    }

    public static readonly DependencyProperty GlyphProperty =
        DependencyProperty.Register("Glyph", typeof(string), typeof(SpeedIndicator), new PropertyMetadata("\uE74A"));

    public string Glyph
    {
        get => (string)GetValue(GlyphProperty);
        set => SetValue(GlyphProperty, value);
    }
}
