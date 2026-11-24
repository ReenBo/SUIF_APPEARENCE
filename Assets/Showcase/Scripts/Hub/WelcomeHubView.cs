using SUIF.API;
using SUIF.Attributes;
using SUIF.Base;
using SUIF.Binding;
using SUIF.R3.Extensions;
using SUIF.Showcase.Contracts;
using UnityEngine;
using UnityEngine.UIElements;

namespace SUIF.Showcase.Hub
{
    [UIView("WelcomeHubView", UILayer.Screens, UILoadingMode.Cached)]
    public sealed class WelcomeHubView : BaseView<WelcomeHubViewModel>
    {
        private const string ToastVisibleClass = "is-visible";
        private const string CardHoveredClass = "is-hovered";
        private const int CardCount = 4;

        [UQuery("hub-stage")] private VisualElement _hubStage;
        [UQuery("hub-backdrop")] private VisualElement _hubBackdrop;
        [UQuery("hub-content")] private VisualElement _hubContent;
        [UQuery("hub-grid")] private VisualElement _hubGrid;

        [UQuery("card-cyberpunk")] private Button _cardCyberpunk;
        [UQuery("card-macos")] private Button _cardMacos;
        [UQuery("card-future1")] private Button _cardFuture1;
        [UQuery("card-future2")] private Button _cardFuture2;

        [UQuery("emblem-cyberpunk")] private VisualElement _emblemCyberpunk;
        [UQuery("emblem-macos")] private VisualElement _emblemMacos;
        [UQuery("emblem-future1")] private VisualElement _emblemFuture1;
        [UQuery("emblem-future2")] private VisualElement _emblemFuture2;

        [UQuery("hub-eyebrow-dot")] private VisualElement _eyebrowDot;
        [UQuery("hub-pill-toast")] private VisualElement _pillToast;
        [UQuery("hub-pill-toast-text")] private Label _pillToastText;

        private readonly Button[] _cards = new Button[CardCount];
        private readonly bool[] _cardHovered = new bool[CardCount];
        private readonly float[] _cardScale = new float[CardCount];
        private readonly float[] _cardMagX = new float[CardCount];
        private readonly float[] _cardMagY = new float[CardCount];
        private readonly float[] _cardTargetMagX = new float[CardCount];
        private readonly float[] _cardTargetMagY = new float[CardCount];
        private readonly float[] _cardPunchElapsed = new float[CardCount];
        private readonly bool[] _cardPunchActive = new bool[CardCount];

        private HubBackdropEffect _backdropEffect;
        private FixedTextSlot _toastTextSlot;
        private IVisualElementScheduledItem _animScheduler;

        private float _targetParallaxX;
        private float _targetParallaxY;
        private float _currentParallaxX;
        private float _currentParallaxY;
        private float _pointerPxX = -9999f;
        private float _pointerPxY = -9999f;
        private float _elapsedTime;
        private double _lastTickTime;
        private bool _isTransitioningOut;
        private float _transitionElapsed;

        public WelcomeHubView(VisualElement root) : base(root)
        {
        }

        protected override void Bind()
        {
            _cards[0] = _cardCyberpunk;
            _cards[1] = _cardMacos;
            _cards[2] = _cardFuture1;
            _cards[3] = _cardFuture2;

            for (int i = 0; i < CardCount; i++)
            {
                _cardScale[i] = 1f;
            }

            if (_hubBackdrop != null)
            {
                _backdropEffect = new HubBackdropEffect(_hubBackdrop);
                AddDisposable(_backdropEffect);
            }

            RegisterEmblems();

            if (_cardCyberpunk != null)
            {
                Binder.Bind(_cardCyberpunk).ToClick(() => BeginExitTransition(true));
                RegisterCardPointerHandlers(_cardCyberpunk, 0);
            }

            if (_cardMacos != null)
            {
                Binder.Bind(_cardMacos).ToClick(() => BeginExitTransition(false));
                RegisterCardPointerHandlers(_cardMacos, 1);
            }

            if (_cardFuture1 != null)
            {
                Binder.Bind(_cardFuture1).ToClick(() => TriggerFutureClick(2, true));
                RegisterCardPointerHandlers(_cardFuture1, 2);
            }

            if (_cardFuture2 != null)
            {
                Binder.Bind(_cardFuture2).ToClick(() => TriggerFutureClick(3, false));
                RegisterCardPointerHandlers(_cardFuture2, 3);
            }

            if (_hubStage != null)
            {
                _hubStage.RegisterCallback<PointerMoveEvent>(OnStagePointerMove);
                _hubStage.RegisterCallback<PointerLeaveEvent>(OnStagePointerLeave);
            }

            _toastTextSlot = Binder.FixedSlot(_pillToastText, 40);
            Binder.ToData(ViewModel.ToastState, this, static (dto, view) => view.ApplyToast(dto));

            _lastTickTime = Time.realtimeSinceStartupAsDouble;
            _animScheduler = VisualElement.schedule.Execute(TickFrame);
            _animScheduler.Every(16);
        }

        public override void Show()
        {
            base.Show();
            _isTransitioningOut = false;
            _transitionElapsed = 0f;
            for (int i = 0; i < CardCount; i++)
            {
                _cardHovered[i] = false;
                _cardTargetMagX[i] = 0f;
                _cardTargetMagY[i] = 0f;
                _cards[i]?.RemoveFromClassList(CardHoveredClass);
            }

            if (_hubContent != null)
            {
                _hubContent.style.opacity = 1f;
                _hubContent.style.scale = new Scale(Vector3.one);
            }

            _lastTickTime = Time.realtimeSinceStartupAsDouble;
            _animScheduler?.Resume();
        }

        public override void Hide()
        {
            _animScheduler?.Pause();
            base.Hide();
        }

        public override void Unbind()
        {
            CleanupCallbacks();
            base.Unbind();
        }

        public override void Dispose()
        {
            CleanupCallbacks();
            base.Dispose();
        }

        private void CleanupCallbacks()
        {
            if (_hubStage != null)
            {
                _hubStage.UnregisterCallback<PointerMoveEvent>(OnStagePointerMove);
                _hubStage.UnregisterCallback<PointerLeaveEvent>(OnStagePointerLeave);
            }

            for (int i = 0; i < CardCount; i++)
            {
                UnregisterCardPointerHandlers(_cards[i]);
            }

            _animScheduler?.Pause();
        }

        private void RegisterEmblems()
        {
            if (_emblemCyberpunk != null)
            {
                AddDisposable(new HubVectorIconEffect(
                    _emblemCyberpunk,
                    HubVectorIconKind.CyberPunkReticle,
                    new Color(0.40f, 0.91f, 0.98f, 1f),
                    new Color(1f, 0.165f, 0.329f, 1f)));
            }

            if (_emblemMacos != null)
            {
                AddDisposable(new HubVectorIconEffect(
                    _emblemMacos,
                    HubVectorIconKind.MacOSTahoeWindows,
                    new Color(0.49f, 0.83f, 0.99f, 1f),
                    new Color(0.22f, 0.74f, 0.97f, 0.20f)));
            }

            if (_emblemFuture1 != null)
            {
                AddDisposable(new HubVectorIconEffect(
                    _emblemFuture1,
                    HubVectorIconKind.FutureDiamond,
                    new Color(1f, 1f, 1f, 0.65f),
                    new Color(0.655f, 0.545f, 0.98f, 0.85f)));
            }

            if (_emblemFuture2 != null)
            {
                AddDisposable(new HubVectorIconEffect(
                    _emblemFuture2,
                    HubVectorIconKind.FuturePrism,
                    new Color(1f, 1f, 1f, 0.65f),
                    new Color(0.655f, 0.545f, 0.98f, 0.85f)));
            }
        }

        private void BeginExitTransition(bool openCyberpunk)
        {
            if (_isTransitioningOut)
            {
                return;
            }

            _isTransitioningOut = true;
            _transitionElapsed = 0f;
            if (openCyberpunk)
            {
                ViewModel.OpenCyberPunk();
            }
            else
            {
                ViewModel.OpenMacOSTahoe();
            }
        }

        private void TriggerFutureClick(int index, bool isFirst)
        {
            _cardPunchActive[index] = true;
            _cardPunchElapsed[index] = 0f;
            if (isFirst)
            {
                ViewModel.TriggerFuture1();
            }
            else
            {
                ViewModel.TriggerFuture2();
            }
        }

        private void RegisterCardPointerHandlers(Button card, int index)
        {
            card.userData = index;
            card.RegisterCallback<PointerEnterEvent>(OnCardPointerEnter);
            card.RegisterCallback<PointerMoveEvent>(OnCardPointerMove);
            card.RegisterCallback<PointerLeaveEvent>(OnCardPointerLeave);
        }

        private void UnregisterCardPointerHandlers(Button card)
        {
            if (card == null)
            {
                return;
            }

            card.UnregisterCallback<PointerEnterEvent>(OnCardPointerEnter);
            card.UnregisterCallback<PointerMoveEvent>(OnCardPointerMove);
            card.UnregisterCallback<PointerLeaveEvent>(OnCardPointerLeave);
        }

        private void OnCardPointerEnter(PointerEnterEvent evt)
        {
            if (evt.currentTarget is Button btn && btn.userData is int idx && idx >= 0 && idx < CardCount)
            {
                _cardHovered[idx] = true;
                btn.AddToClassList(CardHoveredClass);
            }
        }

        private void OnCardPointerMove(PointerMoveEvent evt)
        {
            if (evt.currentTarget is Button btn && btn.userData is int idx && idx >= 0 && idx < CardCount)
            {
                var rect = btn.contentRect;
                if (rect.width > 1f && rect.height > 1f)
                {
                    float nx = Mathf.Clamp((evt.localPosition.x / rect.width) * 2f - 1f, -1f, 1f);
                    float ny = Mathf.Clamp((evt.localPosition.y / rect.height) * 2f - 1f, -1f, 1f);
                    _cardTargetMagX[idx] = nx * 4.5f;
                    _cardTargetMagY[idx] = ny * 4.5f;
                }
            }
        }

        private void OnCardPointerLeave(PointerLeaveEvent evt)
        {
            if (evt.currentTarget is Button btn && btn.userData is int idx && idx >= 0 && idx < CardCount)
            {
                _cardHovered[idx] = false;
                _cardTargetMagX[idx] = 0f;
                _cardTargetMagY[idx] = 0f;
                btn.RemoveFromClassList(CardHoveredClass);
            }
        }

        private void OnStagePointerMove(PointerMoveEvent evt)
        {
            if (_hubStage == null)
            {
                return;
            }

            var rect = _hubStage.contentRect;
            if (rect.width <= 1f || rect.height <= 1f)
            {
                return;
            }

            _pointerPxX = evt.localPosition.x;
            _pointerPxY = evt.localPosition.y;
            _targetParallaxX = Mathf.Clamp((_pointerPxX / rect.width) * 2f - 1f, -1f, 1f);
            _targetParallaxY = Mathf.Clamp((_pointerPxY / rect.height) * 2f - 1f, -1f, 1f);
        }

        private void OnStagePointerLeave(PointerLeaveEvent evt)
        {
            _targetParallaxX = 0f;
            _targetParallaxY = 0f;
            _pointerPxX = -9999f;
            _pointerPxY = -9999f;
        }

        private void ApplyToast(HubToastDto dto)
        {
            if (_pillToast == null)
            {
                return;
            }

            _toastTextSlot?.SetWritten(dto.Slot, ViewModel.Formatter.HubToastText);
            if (dto.IsVisible)
            {
                _pillToast.AddToClassList(ToastVisibleClass);
                _pillToast.style.opacity = 1f;
                _pillToast.style.translate = new Translate(0f, 0f);
            }
            else
            {
                _pillToast.RemoveFromClassList(ToastVisibleClass);
                _pillToast.style.opacity = 0f;
                _pillToast.style.translate = new Translate(0f, 10f);
            }
        }

        private void TickFrame()
        {
            double now = Time.realtimeSinceStartupAsDouble;
            float dt = Mathf.Clamp((float)(now - _lastTickTime), 0.001f, 0.05f);
            _lastTickTime = now;
            _elapsedTime += dt;

            float lerpFactor = 1f - Mathf.Exp(-7.5f * dt);
            _currentParallaxX = Mathf.Lerp(_currentParallaxX, _targetParallaxX, lerpFactor);
            _currentParallaxY = Mathf.Lerp(_currentParallaxY, _targetParallaxY, lerpFactor);

            _backdropEffect?.Step(dt, _currentParallaxX, _currentParallaxY, _pointerPxX, _pointerPxY);

            if (_hubGrid != null)
            {
                _hubGrid.style.translate = new Translate(_currentParallaxX * 5f, _currentParallaxY * 5f);
            }

            if (_eyebrowDot != null)
            {
                float pulse = 0.55f + 0.45f * Mathf.Sin(_elapsedTime * 3.4f);
                _eyebrowDot.style.opacity = pulse;
            }

            float cardLerp = 1f - Mathf.Exp(-12f * dt);
            for (int i = 0; i < CardCount; i++)
            {
                var card = _cards[i];
                if (card == null)
                {
                    continue;
                }

                float floatY = _cardHovered[i] ? 0f : Mathf.Sin(_elapsedTime * 1.8f + i * 1.35f) * 4.5f;
                float tiltDeg = _cardHovered[i] ? _cardMagX[i] * 0.45f : Mathf.Sin(_elapsedTime * 1.35f + i * 1.35f) * 0.6f;

                _cardMagX[i] = Mathf.Lerp(_cardMagX[i], _cardTargetMagX[i], cardLerp);
                _cardMagY[i] = Mathf.Lerp(_cardMagY[i], _cardTargetMagY[i], cardLerp);

                float targetScale = _cardHovered[i] ? 1.045f : 1f;
                _cardScale[i] = Mathf.Lerp(_cardScale[i], targetScale, cardLerp);

                float punchScale = 1f;
                float punchX = 0f;
                if (_cardPunchActive[i])
                {
                    _cardPunchElapsed[i] += dt;
                    float pt = _cardPunchElapsed[i] / 0.42f;
                    if (pt >= 1f)
                    {
                        _cardPunchActive[i] = false;
                    }
                    else
                    {
                        float damp = 1f - pt;
                        punchScale = 1f - Mathf.Sin(pt * Mathf.PI * 2.5f) * 0.065f * damp;
                        punchX = Mathf.Sin(pt * Mathf.PI * 4f) * 5f * damp;
                    }
                }

                float finalScale = _cardScale[i] * punchScale;
                card.style.translate = new Translate(_cardMagX[i] + punchX, floatY + _cardMagY[i]);
                card.style.scale = new Scale(new Vector3(finalScale, finalScale, 1f));
                card.style.rotate = new Rotate(Angle.Degrees(tiltDeg));
            }

            if (_isTransitioningOut && _hubContent != null)
            {
                _transitionElapsed += dt;
                float p = Mathf.Clamp01(_transitionElapsed / 0.24f);
                float ease = 1f - (1f - p) * (1f - p);
                float sc = Mathf.Lerp(1f, 0.95f, ease);
                _hubContent.style.opacity = 1f - ease;
                _hubContent.style.scale = new Scale(new Vector3(sc, sc, 1f));
            }
        }
    }
}
