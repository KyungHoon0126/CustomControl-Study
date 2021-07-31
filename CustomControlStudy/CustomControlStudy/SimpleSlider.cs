using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Shapes;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace CustomControlStudy
{
    /// <summary>
    /// Thumb와 Rectangle이 중요한 역할을 하기 때문에 해당 컨트롤을 코드에서 직접 제어 해야하므로 TemplatePart 속성 추가.
    /// OnApplyTemplate()에서 각 내부 변수들에 GetTemplateChild를 이용해 인스턴스를 가져온다.
    /// </summary>
    [TemplatePart(Name = ThumbPartName, Type = typeof(Thumb))]
    [TemplatePart(Name = RectanglePartName, Type = typeof(Rectangle))]
    public sealed class SimpleSlider : Control
    {
        private const string ThumbPartName = "PART_Thumb";
        private const string RectanglePartName = "PART_Rectangle";

        private Thumb _thumb;
        private Rectangle _rectangle;

        // Default Constructor
        public SimpleSlider()
        {
            DefaultStyleKey = typeof(SimpleSlider);
        }

        // DepedencyProperty Shortcut keys : Ctrl + K, X -> NetFX30 -> Dfine a DepedencyProperty

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        /// <summary>
        /// Value DP
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(double), typeof(SimpleSlider), new PropertyMetadata(0.0, OnValueChanged));

        /// <summary>
        /// Value DP Changed
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var customSlider = (SimpleSlider)d;
            customSlider.UpdateControls();

            // (d as SimpleSlider).UpdateControls();
            // ((SimpleSlider)d).UpdateControls();
        }

        /// <summary>
        /// Maximum
        /// </summary>
        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        /// <summary>
        /// Maximum DP
        /// </summary>
        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register("Maximum", typeof(double), typeof(SimpleSlider), new PropertyMetadata(100.0));

        /// <summary>
        /// Minimum
        /// </summary>
        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        /// <summary>
        /// Minimum DP
        /// </summary>
        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register("Minimum", typeof(double), typeof(SimpleSlider), new PropertyMetadata(0.0));


        /// <summary>
        /// 
        /// </summary>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _thumb = GetTemplateChild(ThumbPartName) as Thumb;
            if (_thumb == null) return;
            _thumb.DragDelta += _thumb_DragDelta;

            _rectangle = GetTemplateChild(RectanglePartName) as Rectangle;
            SizeChanged += SimpleSlider_SizeChanged;
        }

        private void _thumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            var pixelDiff = e.HorizontalChange;
            var currentLeft = Canvas.GetLeft(_thumb);

            if ((currentLeft + pixelDiff) < 0)
            {
                Value = 0;
            }
            else if ((currentLeft + pixelDiff + _thumb.ActualWidth) > ActualWidth)
            {
                Value = Maximum;
            }
            else
            {
                var totalSize = ActualWidth;
                var ratioDiff = pixelDiff / totalSize;
                var rangeSize = Maximum - Minimum;
                var rangeDiff = rangeSize * ratioDiff;
                Value += rangeDiff;
            }
        }

        private void SimpleSlider_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (Math.Abs(e.NewSize.Width - e.PreviousSize.Width) > 0)
            {
                UpdateControls();
            }
        }

        private void UpdateControls()
        {
            double halfTheThumbWith;

            if (_thumb == null) return;

            halfTheThumbWith = _thumb.ActualWidth / 2;
            double totalSize = ActualWidth - halfTheThumbWith * 2;
            double ratio = totalSize / (Maximum - Minimum);

            if (_thumb == null) return;
            Canvas.SetLeft(_thumb, ratio * Value);

            if (_rectangle == null) return;
            _rectangle.Width = ratio * Value + halfTheThumbWith;
        }
    }
}
