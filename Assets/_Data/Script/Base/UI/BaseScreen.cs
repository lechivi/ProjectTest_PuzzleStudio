using UnityEngine;

    public class BaseScreen : BaseUIElement
    {
        public override void Init()
        {
            base.Init();
            this.uiType = UIType.Screen;
        }

        public override void Show(object data)
        {
            base.Show(data);
        }

        public override void Hide()
        {
            base.Hide();
        }
    }

