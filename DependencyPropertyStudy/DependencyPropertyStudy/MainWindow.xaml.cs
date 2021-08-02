using System;
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

namespace DependencyPropertyStudy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// DependencyProperty : 종속속성, 이 속성을 사용하기 위해서는 DependencyObject를 상속 받은 클래스나 컨트롤에서만 사용 가능.
        /// WPF, Silverlight, UWP 앱 등 XAML을 기반으로 하는 개발 환경에서 다양하게 사용.
        /// </summary>


        /// <summary>
        /// DependencyProperty 구성
        /// 1. DependencyProperty : 종속속성은 DependencyProperty를 이야기 한다.
        /// 2. DependencyProperty Identifier : 종속속성 식별자는 IsSpinningProperty를 이야기 한다. 이 식별자는 종속속성의 값을 저장하고, 불러올 때 키 값으로 사용.
        /// 3. CLR(Common Language Runtime) : 종속속성에 값을 넣거나 불러올 때 코드나 XAML에서 실제로 이용. 절대로 내용을 수정하면 안된다.
        /// </summary>

        public bool IsSpinning
        {
            get { return (bool)GetValue(IsSpinningProperty); }
            set { SetValue(IsSpinningProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsSpinning.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsSpinningProperty =
            DependencyProperty.Register("IsSpinning", typeof(bool), typeof(MainWindow), new PropertyMetadata(false));

        // DepedencyProperty GetValue의 동작원리
        // FontSize 찾아 내는 방법.
        // 1) FontSize 요청 -> TextBlock 2) Local 저장소에서 검사 - 없음. 3) VisualTree의 상위 컨트롤에게 문의 - FontSize 프로퍼티 없음.
        // 4) Visaul Tree의 상위 컨트롤인 Grid에게 문의 - FontSize 프로퍼티 없음. 5) VisualTree의 상위 컨트롤인 Window에 문의 - FontSize="30"

        // FontFamily를 찾아 내는 방법.
        // FontFamily 프로퍼티를 설정해주지 않아 VisualTree의 각 컨트롤에게 문의를 해도 없음.
        // 이때, GetValue가 호출이 될 때 값을 찾아서 반환하는 것을 resolved dynamically 이라고 한다.

        /// <summary>
        /// DepedencyProperty 사용 장점
        /// 1) 메모리 사용량 감소 : 누군가 프로퍼티의 값을 요청하면 찾아보고 알려줄 수 있기 때문에 메모리를 더 작게 사용할 수 있다.
        /// 2) 값 상속 : 비주얼 트리의 하위 컨트롤들은 상위 컨트롤이 가지고 있는 프로퍼티의 값을 상속 받을 수 있다. Style들을 이용해 다양한 형태로 값을 상속 시킬 수 있다.
        /// 3) 값 변경 알림 사용 : DepedencyProperty는 값이 변경될 때 MVVM patter에서 처럼 INotifyPropertyChanged 이벤트를 발생 시키며, DataBinding에도 연결할 수 있고, Callback을 등록해 사용할 수 있는 다양한 기능 제공.
        /// </summary>
    }
}
