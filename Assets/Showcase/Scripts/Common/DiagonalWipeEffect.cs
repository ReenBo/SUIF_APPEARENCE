using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace SUIF.Showcase.Common
{
    public enum DiagonalWipeStyle
    {
        Navigation,
        ModalNavigation,
        Close
    }

    public sealed class DiagonalWipeEffect : IDisposable
    {
        private const float Tangent18Deg = 0.3249197f;
        private const float EdgeLeadWidth = 15f;

        private static readonly CustomStyleProperty<Color> FgPrimaryProp = new("--fg-primary");
        private static readonly CustomStyleProperty<Color> CyberCyanProp = new("--cyber-cyan");
        private static readonly CustomStyleProperty<Color> CyberRedProp = new("--cyber-red");

        private static readonly Color DefaultFgPrimary = new(0.95686275f, 0.95686275f, 0.9647059f, 1f);
        private static readonly Color DefaultCyberCyan = new(0f, 0.9411765f, 1f, 1f);
        private static readonly Color DefaultCyberRed = new(1f, 0.16470589f, 0.32941177f, 1f);

        private readonly VisualElement _target;
        private readonly DiagonalWipeStyle _style;
        private readonly float _edgeDuration;
        private readonly float _fillDuration;
        private readonly IVisualElementScheduledItem _scheduler;

        private Color _fgPrimary = DefaultFgPrimary;
        private Color _cyberCyan = DefaultCyberCyan;
        private Color _cyberRed = DefaultCyberRed;

        private bool _isHovered;
        private float _edgeProgress;
        private float _fillProgress;
        private double _lastTickTime;
        private bool _disposed;

        public DiagonalWipeEffect(VisualElement target, DiagonalWipeStyle style)
        {
            _target = target ?? throw new ArgumentNullException(nameof(target));
            _style = style;

            if (style == DiagonalWipeStyle.Close)
            {
                _edgeDuration = 0.28f;
                _fillDuration = 0.32f;
            }
            else
            {
                _edgeDuration = 0.30f;
                _fillDuration = 0.35f;
            }

            _target.RegisterCallback<PointerEnterEvent>(OnPointerEnter);
            _target.RegisterCallback<PointerLeaveEvent>(OnPointerLeave);
            _target.RegisterCallback<CustomStyleResolvedEvent>(OnCustomStyleResolved);
            _target.generateVisualContent += OnGenerateVisualContent;

            _scheduler = _target.schedule.Execute(TickAnimation);
            _scheduler.Every(0);
            _scheduler.Pause();

            ResolveCustomColors(_target.customStyle);
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;
            _scheduler.Pause();
            _target.UnregisterCallback<PointerEnterEvent>(OnPointerEnter);
            _target.UnregisterCallback<PointerLeaveEvent>(OnPointerLeave);
            _target.UnregisterCallback<CustomStyleResolvedEvent>(OnCustomStyleResolved);
            _target.generateVisualContent -= OnGenerateVisualContent;
        }

        private bool IsLockedActive()
        {
            return _style != DiagonalWipeStyle.Close &&
                   (_target.ClassListContains("is-active") || _target.ClassListContains("is-active-modal"));
        }

        private void OnPointerEnter(PointerEnterEvent evt)
        {
            _isHovered = true;
            if (IsLockedActive())
            {
                return;
            }

            _lastTickTime = Time.realtimeSinceStartupAsDouble;
            _scheduler.Resume();
        }

        private void OnPointerLeave(PointerLeaveEvent evt)
        {
            _isHovered = false;
            if (IsLockedActive() || (_edgeProgress <= 0f && _fillProgress <= 0f))
            {
                return;
            }

            _lastTickTime = Time.realtimeSinceStartupAsDouble;
            _scheduler.Resume();
        }

        private void OnCustomStyleResolved(CustomStyleResolvedEvent evt)
        {
            ResolveCustomColors(evt.customStyle);
            if (_edgeProgress > 0f || _fillProgress > 0f)
            {
                _target.MarkDirtyRepaint();
            }
        }

        private void ResolveCustomColors(ICustomStyle customStyle)
        {
            if (customStyle.TryGetValue(FgPrimaryProp, out var fg))
            {
                _fgPrimary = fg;
            }

            if (customStyle.TryGetValue(CyberCyanProp, out var cyan))
            {
                _cyberCyan = cyan;
            }

            if (customStyle.TryGetValue(CyberRedProp, out var red))
            {
                _cyberRed = red;
            }
        }

        private void TickAnimation(TimerState state)
        {
            if (_disposed)
            {
                _scheduler.Pause();
                return;
            }

            if (IsLockedActive())
            {
                _edgeProgress = 0f;
                _fillProgress = 0f;
                _scheduler.Pause();
                _target.MarkDirtyRepaint();
                return;
            }

            var now = Time.realtimeSinceStartupAsDouble;
            var dt = (float)(now - _lastTickTime);
            _lastTickTime = now;
            if (dt <= 0f)
            {
                return;
            }

            if (dt > 0.05f)
            {
                dt = 0.05f;
            }

            var targetVal = _isHovered ? 1f : 0f;
            var stepEdge = dt / _edgeDuration;
            var stepFill = dt / _fillDuration;

            _edgeProgress = _isHovered
                ? Mathf.Min(1f, _edgeProgress + stepEdge)
                : Mathf.Max(0f, _edgeProgress - stepEdge);
            _fillProgress = _isHovered
                ? Mathf.Min(1f, _fillProgress + stepFill)
                : Mathf.Max(0f, _fillProgress - stepFill);

            _target.MarkDirtyRepaint();

            if (_edgeProgress == targetVal && _fillProgress == targetVal)
            {
                _scheduler.Pause();
            }
        }

        private void OnGenerateVisualContent(MeshGenerationContext mgc)
        {
            if (IsLockedActive())
            {
                _edgeProgress = 0f;
                _fillProgress = 0f;
                return;
            }

            if (_edgeProgress <= 0f && _fillProgress <= 0f)
            {
                return;
            }

            var layout = _target.layout;
            var style = _target.resolvedStyle;
            var left = style.borderLeftWidth;
            var top = style.borderTopWidth;
            var right = layout.width - style.borderRightWidth;
            var bottom = layout.height - style.borderBottomWidth;
            var height = bottom - top;

            if (right <= left || height <= 0f)
            {
                return;
            }

            var slant = height * Tangent18Deg;
            var fillT = EaseOutCubic(_fillProgress);
            var edgeT = EaseOutCubic(_edgeProgress);

            var fillXTop = Mathf.Lerp(left - slant - EdgeLeadWidth, right, fillT);
            var edgeXTop = Mathf.Lerp(left - slant, right + EdgeLeadWidth, edgeT);

            Span<Vector2> fillPoly = stackalloc Vector2[6];
            Span<Vector2> edgePoly = stackalloc Vector2[6];

            var fillCount = _fillProgress > 0f
                ? ClipBand(left, top, right, bottom, float.NegativeInfinity, fillXTop, slant, fillPoly)
                : 0;
            var edgeCount = _edgeProgress > 0f && edgeXTop > fillXTop
                ? ClipBand(left, top, right, bottom, fillXTop, edgeXTop, slant, edgePoly)
                : 0;

            var totalVerts = fillCount + edgeCount;
            if (totalVerts == 0)
            {
                return;
            }

            var totalIndices = (fillCount >= 3 ? (fillCount - 2) * 3 : 0) +
                               (edgeCount >= 3 ? (edgeCount - 2) * 3 : 0);
            if (totalIndices == 0)
            {
                return;
            }

            GetWipeColors(out var edgeColor, out var fillColor);
            var mesh = mgc.Allocate(totalVerts, totalIndices);

            if (fillCount >= 3)
            {
                WritePolygon(mesh, fillPoly, fillCount, fillColor, 0);
            }

            if (edgeCount >= 3)
            {
                WritePolygon(mesh, edgePoly, edgeCount, edgeColor, fillCount);
            }
        }

        private void GetWipeColors(out Color32 edgeColor, out Color32 fillColor)
        {
            switch (_style)
            {
                case DiagonalWipeStyle.ModalNavigation:
                    edgeColor = _cyberRed;
                    fillColor = _cyberCyan;
                    break;
                case DiagonalWipeStyle.Close:
                    edgeColor = _cyberCyan;
                    fillColor = _cyberRed;
                    break;
                default:
                    edgeColor = _cyberCyan;
                    fillColor = _fgPrimary;
                    break;
            }
        }

        private static float EaseOutCubic(float t)
        {
            var inv = 1f - t;
            return 1f - inv * inv * inv;
        }

        private static void WritePolygon(MeshWriteData mesh, Span<Vector2> poly, int count, Color32 color, int baseVertex)
        {
            for (var i = 0; i < count; i++)
            {
                mesh.SetNextVertex(new Vertex
                {
                    position = new Vector3(poly[i].x, poly[i].y, Vertex.nearZ),
                    tint = color
                });
            }

            for (var i = 1; i < count - 1; i++)
            {
                mesh.SetNextIndex((ushort)baseVertex);
                mesh.SetNextIndex((ushort)(baseVertex + i));
                mesh.SetNextIndex((ushort)(baseVertex + i + 1));
            }
        }

        private static int ClipBand(
            float left,
            float top,
            float right,
            float bottom,
            float minXAtTop,
            float maxXAtTop,
            float slant,
            Span<Vector2> output)
        {
            if (maxXAtTop + slant <= left || minXAtTop >= right)
            {
                return 0;
            }

            Span<Vector2> bufA = stackalloc Vector2[6];
            bufA[0] = new Vector2(left, top);
            bufA[1] = new Vector2(right, top);
            bufA[2] = new Vector2(right, bottom);
            bufA[3] = new Vector2(left, bottom);
            var count = 4;

            Span<Vector2> bufB = stackalloc Vector2[6];
            count = ClipAgainstDiagonal(bufA, count, bufB, top, maxXAtTop, keepLeft: true);
            if (count < 3)
            {
                return 0;
            }

            if (minXAtTop + slant > left)
            {
                count = ClipAgainstDiagonal(bufB, count, output, top, minXAtTop, keepLeft: false);
                return count >= 3 ? count : 0;
            }

            for (var i = 0; i < count; i++)
            {
                output[i] = bufB[i];
            }

            return count;
        }

        private static int ClipAgainstDiagonal(
            Span<Vector2> input,
            int inCount,
            Span<Vector2> output,
            float top,
            float xAtTop,
            bool keepLeft)
        {
            var outCount = 0;
            var prev = input[inCount - 1];
            var prevDist = SignedDist(prev, top, xAtTop, keepLeft);
            var prevInside = prevDist <= 0f;

            for (var i = 0; i < inCount; i++)
            {
                var curr = input[i];
                var currDist = SignedDist(curr, top, xAtTop, keepLeft);
                var currInside = currDist <= 0f;

                if (currInside != prevInside)
                {
                    var denom = prevDist - currDist;
                    var t = Mathf.Abs(denom) > 1e-6f ? prevDist / denom : 0f;
                    output[outCount++] = Vector2.Lerp(prev, curr, t);
                }

                if (currInside)
                {
                    output[outCount++] = curr;
                }

                prev = curr;
                prevDist = currDist;
                prevInside = currInside;
            }

            return outCount;
        }

        private static float SignedDist(Vector2 point, float top, float xAtTop, bool keepLeft)
        {
            var lineX = xAtTop + (point.y - top) * Tangent18Deg;
            return keepLeft ? point.x - lineX : lineX - point.x;
        }
    }
}

