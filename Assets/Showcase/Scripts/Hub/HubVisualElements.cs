using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace SUIF.Showcase.Hub
{
    public enum HubVectorIconKind : byte
    {
        CyberPunkReticle,
        MacOSTahoeWindows,
        FutureDiamond,
        FuturePrism,
        TahoeLifecycle,
        TahoeCounter,
        TahoeRecycledList,
        TahoeModals,
        TahoeInv6,
        TahoeInv100,
        TahoeThemes,
        TahoeDesktopCommand
    }

    public sealed class HubVectorIconEffect : IDisposable
    {
        private readonly VisualElement _target;
        private readonly HubVectorIconKind _kind;
        private readonly Color _primaryColor;
        private readonly Color _accentColor;
        private bool _disposed;

        public HubVectorIconEffect(
            VisualElement target,
            HubVectorIconKind kind,
            Color primaryColor,
            Color accentColor)
        {
            _target = target ?? throw new ArgumentNullException(nameof(target));
            _kind = kind;
            _primaryColor = primaryColor;
            _accentColor = accentColor;
            _target.generateVisualContent += OnGenerateVisualContent;
            _target.MarkDirtyRepaint();
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;
            _target.generateVisualContent -= OnGenerateVisualContent;
        }

        private void OnGenerateVisualContent(MeshGenerationContext mgc)
        {
            var rect = _target.contentRect;
            if (rect.width <= 1f || rect.height <= 1f)
            {
                return;
            }

            var painter = mgc.painter2D;
            float s = Mathf.Min(rect.width, rect.height) / 26f;
            float ox = (rect.width - 26f * s) * 0.5f;
            float oy = (rect.height - 26f * s) * 0.5f;

            switch (_kind)
            {
                case HubVectorIconKind.CyberPunkReticle:
                    DrawCyberPunkReticle(painter, ox, oy, s);
                    break;
                case HubVectorIconKind.MacOSTahoeWindows:
                    DrawMacOSTahoeWindows(painter, ox, oy, s);
                    break;
                case HubVectorIconKind.FutureDiamond:
                    DrawFutureDiamond(painter, ox, oy, s);
                    break;
                case HubVectorIconKind.FuturePrism:
                    DrawFuturePrism(painter, ox, oy, s);
                    break;
                case HubVectorIconKind.TahoeLifecycle:
                    DrawTahoeLifecycle(painter, ox, oy, s);
                    break;
                case HubVectorIconKind.TahoeCounter:
                    DrawTahoeCounter(painter, ox, oy, s);
                    break;
                case HubVectorIconKind.TahoeRecycledList:
                    DrawTahoeRecycledList(painter, ox, oy, s);
                    break;
                case HubVectorIconKind.TahoeModals:
                    DrawTahoeModals(painter, ox, oy, s);
                    break;
                case HubVectorIconKind.TahoeInv6:
                    DrawTahoeInv6(painter, ox, oy, s);
                    break;
                case HubVectorIconKind.TahoeInv100:
                    DrawTahoeInv100(painter, ox, oy, s);
                    break;
                case HubVectorIconKind.TahoeThemes:
                    DrawTahoeThemes(painter, ox, oy, s);
                    break;
                case HubVectorIconKind.TahoeDesktopCommand:
                    DrawTahoeCommand(painter, ox, oy, s);
                    break;
            }
        }

        private void DrawCyberPunkReticle(Painter2D painter, float ox, float oy, float s)
        {
            painter.strokeColor = _primaryColor;
            painter.lineWidth = 1.6f * s;
            painter.BeginPath();
            painter.MoveTo(Pt(ox, oy, s, 4f, 2f));
            painter.LineTo(Pt(ox, oy, s, 20f, 2f));
            painter.LineTo(Pt(ox, oy, s, 24f, 6f));
            painter.LineTo(Pt(ox, oy, s, 24f, 22f));
            painter.LineTo(Pt(ox, oy, s, 6f, 22f));
            painter.LineTo(Pt(ox, oy, s, 2f, 18f));
            painter.ClosePath();
            painter.Stroke();

            painter.strokeColor = _accentColor;
            painter.BeginPath();
            painter.Arc(Pt(ox, oy, s, 13f, 12f), 4f * s, 0f, 360f);
            painter.Stroke();

            painter.strokeColor = _primaryColor;
            painter.BeginPath();
            painter.MoveTo(Pt(ox, oy, s, 13f, 5f));
            painter.LineTo(Pt(ox, oy, s, 13f, 7.5f));
            painter.MoveTo(Pt(ox, oy, s, 13f, 16.5f));
            painter.LineTo(Pt(ox, oy, s, 13f, 19f));
            painter.MoveTo(Pt(ox, oy, s, 6f, 12f));
            painter.LineTo(Pt(ox, oy, s, 8.5f, 12f));
            painter.MoveTo(Pt(ox, oy, s, 17.5f, 12f));
            painter.LineTo(Pt(ox, oy, s, 20f, 12f));
            painter.Stroke();
        }

        private void DrawMacOSTahoeWindows(Painter2D painter, float ox, float oy, float s)
        {
            var backStroke = _primaryColor;
            backStroke.a *= 0.55f;
            painter.strokeColor = backStroke;
            painter.lineWidth = 1.5f * s;
            DrawRoundedRect(painter, ox, oy, s, 3f, 4f, 15f, 12f, 3f);
            painter.Stroke();

            painter.fillColor = _accentColor;
            DrawRoundedRect(painter, ox, oy, s, 8f, 9f, 15f, 13f, 3f);
            painter.Fill();

            painter.strokeColor = _primaryColor;
            DrawRoundedRect(painter, ox, oy, s, 8f, 9f, 15f, 13f, 3f);
            painter.Stroke();

            painter.fillColor = _primaryColor;
            DrawDot(painter, ox, oy, s, 11.2f, 12.2f, 1.0f);
            DrawDot(painter, ox, oy, s, 13.8f, 12.2f, 1.0f);
            DrawDot(painter, ox, oy, s, 16.4f, 12.2f, 1.0f);
        }

        private void DrawFutureDiamond(Painter2D painter, float ox, float oy, float s)
        {
            painter.strokeColor = _primaryColor;
            painter.lineWidth = 1.5f * s;
            DrawDashedSegment(painter, ox, oy, s, 13f, 3f, 23f, 13f, 3);
            DrawDashedSegment(painter, ox, oy, s, 23f, 13f, 13f, 23f, 3);
            DrawDashedSegment(painter, ox, oy, s, 13f, 23f, 3f, 13f, 3);
            DrawDashedSegment(painter, ox, oy, s, 3f, 13f, 13f, 3f, 3);

            painter.fillColor = _accentColor;
            DrawDot(painter, ox, oy, s, 13f, 13f, 2.2f);
        }

        private void DrawFuturePrism(Painter2D painter, float ox, float oy, float s)
        {
            painter.strokeColor = _primaryColor;
            painter.lineWidth = 1.5f * s;
            DrawDashedSegment(painter, ox, oy, s, 13f, 3.5f, 22f, 19.5f, 3);
            DrawDashedSegment(painter, ox, oy, s, 22f, 19.5f, 4f, 19.5f, 3);
            DrawDashedSegment(painter, ox, oy, s, 4f, 19.5f, 13f, 3.5f, 3);

            var axis = _primaryColor;
            axis.a *= 0.45f;
            painter.strokeColor = axis;
            painter.BeginPath();
            painter.MoveTo(Pt(ox, oy, s, 13f, 3.5f));
            painter.LineTo(Pt(ox, oy, s, 13f, 19.5f));
            painter.Stroke();

            painter.fillColor = _accentColor;
            DrawDot(painter, ox, oy, s, 13f, 14f, 2.0f);
        }

        private void DrawTahoeLifecycle(Painter2D painter, float ox, float oy, float s)
        {
            painter.strokeColor = _primaryColor;
            painter.lineWidth = 2.0f * s;
            painter.BeginPath();
            painter.Arc(Pt(ox, oy, s, 13f, 13f), 7f * s, 35f, 315f);
            painter.Stroke();

            painter.BeginPath();
            painter.MoveTo(Pt(ox, oy, s, 18f, 6f));
            painter.LineTo(Pt(ox, oy, s, 19.5f, 10.5f));
            painter.LineTo(Pt(ox, oy, s, 15f, 10.5f));
            painter.ClosePath();
            painter.fillColor = _primaryColor;
            painter.Fill();
        }

        private void DrawTahoeCounter(Painter2D painter, float ox, float oy, float s)
        {
            painter.strokeColor = _primaryColor;
            painter.lineWidth = 2.0f * s;
            painter.BeginPath();
            painter.MoveTo(Pt(ox, oy, s, 14.5f, 4f));
            painter.LineTo(Pt(ox, oy, s, 8f, 14f));
            painter.LineTo(Pt(ox, oy, s, 13f, 14f));
            painter.LineTo(Pt(ox, oy, s, 11.5f, 22f));
            painter.LineTo(Pt(ox, oy, s, 18f, 12f));
            painter.LineTo(Pt(ox, oy, s, 13f, 12f));
            painter.ClosePath();
            painter.Stroke();
        }

        private void DrawTahoeRecycledList(Painter2D painter, float ox, float oy, float s)
        {
            painter.strokeColor = _primaryColor;
            painter.lineWidth = 2.0f * s;
            painter.BeginPath();
            painter.MoveTo(Pt(ox, oy, s, 6f, 8f));
            painter.LineTo(Pt(ox, oy, s, 20f, 8f));
            painter.MoveTo(Pt(ox, oy, s, 6f, 13f));
            painter.LineTo(Pt(ox, oy, s, 20f, 13f));
            painter.MoveTo(Pt(ox, oy, s, 6f, 18f));
            painter.LineTo(Pt(ox, oy, s, 20f, 18f));
            painter.Stroke();
        }

        private void DrawTahoeModals(Painter2D painter, float ox, float oy, float s)
        {
            painter.strokeColor = _primaryColor;
            painter.lineWidth = 1.8f * s;
            painter.BeginPath();
            painter.MoveTo(Pt(ox, oy, s, 13f, 5f));
            painter.LineTo(Pt(ox, oy, s, 21f, 13f));
            painter.LineTo(Pt(ox, oy, s, 13f, 21f));
            painter.LineTo(Pt(ox, oy, s, 5f, 13f));
            painter.ClosePath();
            painter.Stroke();

            painter.fillColor = _primaryColor;
            DrawDot(painter, ox, oy, s, 13f, 13f, 2.2f);
        }

        private void DrawTahoeInv6(Painter2D painter, float ox, float oy, float s)
        {
            painter.strokeColor = _primaryColor;
            painter.lineWidth = 1.8f * s;
            DrawRoundedRect(painter, ox, oy, s, 5f, 5f, 7f, 7f, 1.5f);
            painter.Stroke();
            DrawRoundedRect(painter, ox, oy, s, 14f, 5f, 7f, 7f, 1.5f);
            painter.Stroke();
            DrawRoundedRect(painter, ox, oy, s, 5f, 14f, 7f, 7f, 1.5f);
            painter.Stroke();
            DrawRoundedRect(painter, ox, oy, s, 14f, 14f, 7f, 7f, 1.5f);
            painter.Stroke();
        }

        private void DrawTahoeInv100(Painter2D painter, float ox, float oy, float s)
        {
            painter.strokeColor = _primaryColor;
            painter.lineWidth = 1.8f * s;
            DrawRoundedRect(painter, ox, oy, s, 5f, 5f, 16f, 16f, 2.5f);
            painter.Stroke();

            painter.BeginPath();
            painter.MoveTo(Pt(ox, oy, s, 13f, 5f));
            painter.LineTo(Pt(ox, oy, s, 13f, 21f));
            painter.MoveTo(Pt(ox, oy, s, 5f, 13f));
            painter.LineTo(Pt(ox, oy, s, 21f, 13f));
            painter.Stroke();
        }

        private void DrawTahoeThemes(Painter2D painter, float ox, float oy, float s)
        {
            painter.strokeColor = _primaryColor;
            painter.lineWidth = 1.8f * s;
            painter.BeginPath();
            painter.Arc(Pt(ox, oy, s, 13f, 13f), 7.5f * s, 0f, 360f);
            painter.Stroke();

            painter.fillColor = _primaryColor;
            painter.BeginPath();
            painter.Arc(Pt(ox, oy, s, 13f, 13f), 7.5f * s, -90f, 90f);
            painter.ClosePath();
            painter.Fill();
        }

        private void DrawTahoeCommand(Painter2D painter, float ox, float oy, float s)
        {
            painter.strokeColor = _primaryColor;
            painter.lineWidth = 1.8f * s;
            DrawRoundedRect(painter, ox, oy, s, 8.5f, 8.5f, 9f, 9f, 1.5f);
            painter.Stroke();
            DrawRoundedRect(painter, ox, oy, s, 4f, 4f, 4.5f, 4.5f, 2.2f);
            painter.Stroke();
            DrawRoundedRect(painter, ox, oy, s, 17.5f, 4f, 4.5f, 4.5f, 2.2f);
            painter.Stroke();
            DrawRoundedRect(painter, ox, oy, s, 4f, 17.5f, 4.5f, 4.5f, 2.2f);
            painter.Stroke();
            DrawRoundedRect(painter, ox, oy, s, 17.5f, 17.5f, 4.5f, 4.5f, 2.2f);
            painter.Stroke();
        }

        private static Vector2 Pt(float ox, float oy, float s, float x, float y)
        {
            return new Vector2(ox + x * s, oy + y * s);
        }

        private static void DrawDot(Painter2D painter, float ox, float oy, float s, float x, float y, float r)
        {
            painter.BeginPath();
            painter.Arc(Pt(ox, oy, s, x, y), r * s, 0f, 360f);
            painter.Fill();
        }

        private static void DrawRoundedRect(Painter2D painter, float ox, float oy, float s, float x, float y, float w, float h, float r)
        {
            float cr = Mathf.Min(r, Mathf.Min(w, h) * 0.5f);
            painter.BeginPath();
            painter.MoveTo(Pt(ox, oy, s, x + cr, y));
            painter.LineTo(Pt(ox, oy, s, x + w - cr, y));
            painter.LineTo(Pt(ox, oy, s, x + w, y + cr));
            painter.LineTo(Pt(ox, oy, s, x + w, y + h - cr));
            painter.LineTo(Pt(ox, oy, s, x + w - cr, y + h));
            painter.LineTo(Pt(ox, oy, s, x + cr, y + h));
            painter.LineTo(Pt(ox, oy, s, x, y + h - cr));
            painter.LineTo(Pt(ox, oy, s, x, y + cr));
            painter.ClosePath();
        }

        private static void DrawDashedSegment(Painter2D painter, float ox, float oy, float s, float x1, float y1, float x2, float y2, int segments)
        {
            for (int i = 0; i < segments; i++)
            {
                float t0 = (float)i / segments;
                float t1 = t0 + 0.58f / segments;
                float sx = Mathf.Lerp(x1, x2, t0);
                float sy = Mathf.Lerp(y1, y2, t0);
                float ex = Mathf.Lerp(x1, x2, t1);
                float ey = Mathf.Lerp(y1, y2, t1);
                painter.BeginPath();
                painter.MoveTo(Pt(ox, oy, s, sx, sy));
                painter.LineTo(Pt(ox, oy, s, ex, ey));
                painter.Stroke();
            }
        }
    }

    public sealed class HubBackdropEffect : IDisposable
    {
        private const int OrbCount = 4;
        private const int OrbSegments = 24;
        private const int OrbRingCount = 2;
        private const int VertsPerOrb = 1 + OrbSegments * OrbRingCount;
        private const int IndicesPerOrb = OrbSegments * 3 + OrbSegments * 6;
        private const int MaxGridLines = 64;
        private const int ParticleCount = 48;
        private const int MaxFilaments = 80;

        private const int TotalBackdropVerts =
            OrbCount * VertsPerOrb +
            MaxGridLines * 4 +
            ParticleCount * 4 +
            MaxFilaments * 4;

        private const int TotalBackdropIndices =
            OrbCount * IndicesPerOrb +
            MaxGridLines * 6 +
            ParticleCount * 6 +
            MaxFilaments * 6;

        private struct Particle
        {
            public float NX;
            public float NY;
            public float VX;
            public float VY;
            public float Radius;
            public float Depth;
            public float Phase;
            public Color32 Color;
            public bool IsDiamond;
        }

        private struct ProjectedParticle
        {
            public float X;
            public float Y;
            public float Depth;
        }

        private readonly VisualElement _target;
        private readonly Particle[] _particles = new Particle[ParticleCount];
        private readonly ProjectedParticle[] _projected = new ProjectedParticle[ParticleCount];

        private float _time;
        private float _parallaxX;
        private float _parallaxY;
        private float _pointerPxX = -9999f;
        private float _pointerPxY = -9999f;
        private bool _disposed;

        public HubBackdropEffect(VisualElement target)
        {
            _target = target ?? throw new ArgumentNullException(nameof(target));
            InitParticles();
            _target.generateVisualContent += OnGenerateVisualContent;
        }

        public void Step(float dt, float parallaxX, float parallaxY, float pointerPxX, float pointerPxY)
        {
            if (_disposed)
            {
                return;
            }

            _time += dt;
            _parallaxX = parallaxX;
            _parallaxY = parallaxY;
            _pointerPxX = pointerPxX;
            _pointerPxY = pointerPxY;

            for (int i = 0; i < ParticleCount; i++)
            {
                _particles[i].NX += _particles[i].VX * dt;
                _particles[i].NY += _particles[i].VY * dt;

                if (_particles[i].NX < -0.02f) _particles[i].NX = 1.02f;
                else if (_particles[i].NX > 1.02f) _particles[i].NX = -0.02f;

                if (_particles[i].NY < -0.02f) _particles[i].NY = 1.02f;
                else if (_particles[i].NY > 1.02f) _particles[i].NY = -0.02f;
            }

            _target.MarkDirtyRepaint();
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;
            _target.generateVisualContent -= OnGenerateVisualContent;
        }

        private void InitParticles()
        {
            var palette = new Color32[]
            {
                new(56, 189, 248, 255),
                new(129, 140, 248, 255),
                new(192, 132, 252, 255),
                new(248, 250, 252, 255)
            };

            uint seed = 0x9E3779B9u;
            for (int i = 0; i < ParticleCount; i++)
            {
                seed = NextHash(seed);
                float nx = (seed & 0xFFFF) / 65535f;
                seed = NextHash(seed);
                float ny = (seed & 0xFFFF) / 65535f;
                seed = NextHash(seed);
                float depth = 0.28f + ((seed & 0xFFFF) / 65535f) * 0.72f;
                seed = NextHash(seed);
                float vx = (((seed & 0xFFFF) / 65535f) - 0.5f) * 0.014f * depth;
                seed = NextHash(seed);
                float vy = (-0.005f - ((seed & 0xFFFF) / 65535f) * 0.012f) * depth;

                _particles[i] = new Particle
                {
                    NX = nx,
                    NY = ny,
                    VX = vx,
                    VY = vy,
                    Radius = 1.2f + depth * 1.75f,
                    Depth = depth,
                    Phase = i * 0.73f,
                    Color = palette[i % palette.Length],
                    IsDiamond = (i & 3) == 0
                };
            }
        }

        private static uint NextHash(uint x)
        {
            x ^= x << 13;
            x ^= x >> 17;
            x ^= x << 5;
            return x;
        }

        private void OnGenerateVisualContent(MeshGenerationContext mgc)
        {
            var rect = _target.contentRect;
            float w = rect.width;
            float h = rect.height;
            if (w <= 4f || h <= 4f)
            {
                return;
            }

            var mesh = mgc.Allocate(TotalBackdropVerts, TotalBackdropIndices);
            int writtenVerts = 0;
            int writtenIndices = 0;

            WriteOrbs(mesh, w, h, ref writtenVerts, ref writtenIndices);
            WriteGrid(mesh, w, h, ref writtenVerts, ref writtenIndices);
            WriteParticlesAndFilaments(mesh, w, h, ref writtenVerts, ref writtenIndices);

            for (int i = writtenVerts; i < TotalBackdropVerts; i++)
            {
                mesh.SetNextVertex(new Vertex { position = Vector3.zero, tint = default });
            }

            for (int i = writtenIndices; i < TotalBackdropIndices; i++)
            {
                mesh.SetNextIndex(0);
            }
        }

        private void WriteOrbs(MeshWriteData mesh, float w, float h, ref int writtenVerts, ref int writtenIndices)
        {
            float baseShiftX = _parallaxX * -24f;
            float baseShiftY = _parallaxY * -24f;
            float t = _time * 0.72f;

            WriteRadialOrb(
                mesh,
                w * 0.24f + baseShiftX + Mathf.Sin(t * 0.7f) * 18f,
                h * 0.24f + baseShiftY + Mathf.Cos(t * 0.6f) * 14f,
                Mathf.Max(w, h) * 0.34f,
                new Color32(79, 70, 229, 98),
                ref writtenVerts,
                ref writtenIndices);

            WriteRadialOrb(
                mesh,
                w * 0.78f + baseShiftX + Mathf.Cos(t * 0.65f) * -16f,
                h * 0.28f + baseShiftY + Mathf.Sin(t * 0.55f) * 16f,
                Mathf.Max(w, h) * 0.32f,
                new Color32(6, 182, 212, 82),
                ref writtenVerts,
                ref writtenIndices);

            WriteRadialOrb(
                mesh,
                w * 0.36f + baseShiftX + Mathf.Sin(t * 0.5f) * 15f,
                h * 0.78f + baseShiftY + Mathf.Cos(t * 0.75f) * -15f,
                Mathf.Max(w, h) * 0.35f,
                new Color32(139, 92, 246, 78),
                ref writtenVerts,
                ref writtenIndices);

            WriteRadialOrb(
                mesh,
                w * 0.74f + baseShiftX + Mathf.Cos(t * 0.8f) * -14f,
                h * 0.74f + baseShiftY + Mathf.Sin(t * 0.6f) * -12f,
                Mathf.Max(w, h) * 0.27f,
                new Color32(245, 158, 11, 48),
                ref writtenVerts,
                ref writtenIndices);
        }

        private static void WriteRadialOrb(
            MeshWriteData mesh,
            float cx,
            float cy,
            float radius,
            Color32 centerColor,
            ref int writtenVerts,
            ref int writtenIndices)
        {
            int centerIdx = writtenVerts;
            mesh.SetNextVertex(new Vertex
            {
                position = new Vector3(cx, cy, Vertex.nearZ),
                tint = centerColor
            });
            writtenVerts++;

            Color32 midColor = new Color32(centerColor.r, centerColor.g, centerColor.b, (byte)(centerColor.a * 0.38f));
            Color32 outerColor = new Color32(centerColor.r, centerColor.g, centerColor.b, 0);

            int ring1Start = writtenVerts;
            float r1 = radius * 0.52f;
            for (int i = 0; i < OrbSegments; i++)
            {
                float angle = (i / (float)OrbSegments) * Mathf.PI * 2f;
                mesh.SetNextVertex(new Vertex
                {
                    position = new Vector3(cx + Mathf.Cos(angle) * r1, cy + Mathf.Sin(angle) * r1, Vertex.nearZ),
                    tint = midColor
                });
                writtenVerts++;
            }

            int ring2Start = writtenVerts;
            for (int i = 0; i < OrbSegments; i++)
            {
                float angle = (i / (float)OrbSegments) * Mathf.PI * 2f;
                mesh.SetNextVertex(new Vertex
                {
                    position = new Vector3(cx + Mathf.Cos(angle) * radius, cy + Mathf.Sin(angle) * radius, Vertex.nearZ),
                    tint = outerColor
                });
                writtenVerts++;
            }

            for (int i = 0; i < OrbSegments; i++)
            {
                int next = (i + 1) % OrbSegments;
                mesh.SetNextIndex((ushort)centerIdx);
                mesh.SetNextIndex((ushort)(ring1Start + i));
                mesh.SetNextIndex((ushort)(ring1Start + next));

                mesh.SetNextIndex((ushort)(ring1Start + i));
                mesh.SetNextIndex((ushort)(ring2Start + i));
                mesh.SetNextIndex((ushort)(ring2Start + next));

                mesh.SetNextIndex((ushort)(ring1Start + i));
                mesh.SetNextIndex((ushort)(ring2Start + next));
                mesh.SetNextIndex((ushort)(ring1Start + next));

                writtenIndices += 9;
            }
        }

        private void WriteGrid(MeshWriteData mesh, float w, float h, ref int writtenVerts, ref int writtenIndices)
        {
            const float cell = 64f;
            float shiftX = _parallaxX * -11f;
            float shiftY = _parallaxY * -11f;
            int linesWritten = 0;

            float startX = Mathf.Repeat(shiftX, cell);
            for (float x = startX; x < w && linesWritten < MaxGridLines; x += cell)
            {
                float dx = Mathf.Abs(x - w * 0.5f) / (w * 0.55f);
                float fade = Mathf.Clamp01(1f - dx * dx);
                byte alpha = (byte)(fade * 12f);
                if (alpha > 0)
                {
                    WriteQuadLine(mesh, x, 0f, x, h, 1f, new Color32(255, 255, 255, alpha), ref writtenVerts, ref writtenIndices);
                    linesWritten++;
                }
            }

            float startY = Mathf.Repeat(shiftY, cell);
            for (float y = startY; y < h && linesWritten < MaxGridLines; y += cell)
            {
                float dy = Mathf.Abs(y - h * 0.5f) / (h * 0.55f);
                float fade = Mathf.Clamp01(1f - dy * dy);
                byte alpha = (byte)(fade * 12f);
                if (alpha > 0)
                {
                    WriteQuadLine(mesh, 0f, y, w, y, 1f, new Color32(255, 255, 255, alpha), ref writtenVerts, ref writtenIndices);
                    linesWritten++;
                }
            }
        }

        private void WriteParticlesAndFilaments(MeshWriteData mesh, float w, float h, ref int writtenVerts, ref int writtenIndices)
        {
            float t = _time;
            for (int i = 0; i < ParticleCount; i++)
            {
                ref readonly var p = ref _particles[i];
                float px = p.NX * w + _parallaxX * 32f * p.Depth;
                float py = p.NY * h + _parallaxY * 32f * p.Depth;
                _projected[i] = new ProjectedParticle { X = px, Y = py, Depth = p.Depth };

                float dx = px - _pointerPxX;
                float dy = py - _pointerPxY;
                float distSq = dx * dx + dy * dy;
                float proximityBoost = distSq < 25600f ? (1f - Mathf.Sqrt(distSq) / 160f) * 0.45f : 0f;

                float twinkle = 0.45f + 0.35f * Mathf.Sin(t * 2.1f + p.Phase);
                float alpha = Mathf.Min(0.95f, (0.24f + p.Depth * 0.42f) * twinkle + proximityBoost);
                byte aByte = (byte)(alpha * 255f);
                var tint = new Color32(p.Color.r, p.Color.g, p.Color.b, aByte);

                float s = p.IsDiamond ? p.Radius * 1.45f : p.Radius;
                int vBase = writtenVerts;
                if (p.IsDiamond)
                {
                    mesh.SetNextVertex(new Vertex { position = new Vector3(px, py - s, Vertex.nearZ), tint = tint });
                    mesh.SetNextVertex(new Vertex { position = new Vector3(px + s, py, Vertex.nearZ), tint = tint });
                    mesh.SetNextVertex(new Vertex { position = new Vector3(px, py + s, Vertex.nearZ), tint = tint });
                    mesh.SetNextVertex(new Vertex { position = new Vector3(px - s, py, Vertex.nearZ), tint = tint });
                }
                else
                {
                    mesh.SetNextVertex(new Vertex { position = new Vector3(px - s, py - s, Vertex.nearZ), tint = tint });
                    mesh.SetNextVertex(new Vertex { position = new Vector3(px + s, py - s, Vertex.nearZ), tint = tint });
                    mesh.SetNextVertex(new Vertex { position = new Vector3(px + s, py + s, Vertex.nearZ), tint = tint });
                    mesh.SetNextVertex(new Vertex { position = new Vector3(px - s, py + s, Vertex.nearZ), tint = tint });
                }

                writtenVerts += 4;
                mesh.SetNextIndex((ushort)vBase);
                mesh.SetNextIndex((ushort)(vBase + 1));
                mesh.SetNextIndex((ushort)(vBase + 2));
                mesh.SetNextIndex((ushort)vBase);
                mesh.SetNextIndex((ushort)(vBase + 2));
                mesh.SetNextIndex((ushort)(vBase + 3));
                writtenIndices += 6;
            }

            float maxDist = w < 640f ? 95f : 125f;
            float maxDistSq = maxDist * maxDist;
            int filamentsWritten = 0;

            for (int i = 0; i < ParticleCount && filamentsWritten < MaxFilaments; i++)
            {
                var a = _projected[i];
                for (int j = i + 1; j < ParticleCount && filamentsWritten < MaxFilaments; j++)
                {
                    var b = _projected[j];
                    if (Mathf.Abs(a.Depth - b.Depth) > 0.35f)
                    {
                        continue;
                    }

                    float dx = a.X - b.X;
                    float dy = a.Y - b.Y;
                    float dSq = dx * dx + dy * dy;
                    if (dSq < maxDistSq)
                    {
                        float ratio = 1f - Mathf.Sqrt(dSq) / maxDist;
                        float lineAlpha = ratio * 0.18f * Mathf.Min(a.Depth, b.Depth);
                        byte aByte = (byte)(Mathf.Clamp01(lineAlpha) * 255f);
                        if (aByte > 2)
                        {
                            WriteQuadLine(mesh, a.X, a.Y, b.X, b.Y, 0.85f, new Color32(148, 210, 255, aByte), ref writtenVerts, ref writtenIndices);
                            filamentsWritten++;
                        }
                    }
                }
            }
        }

        private static void WriteQuadLine(
            MeshWriteData mesh,
            float x0,
            float y0,
            float x1,
            float y1,
            float thickness,
            Color32 color,
            ref int writtenVerts,
            ref int writtenIndices)
        {
            float dx = x1 - x0;
            float dy = y1 - y0;
            float len = Mathf.Sqrt(dx * dx + dy * dy);
            if (len <= 0.001f)
            {
                return;
            }

            float nx = (-dy / len) * (thickness * 0.5f);
            float ny = (dx / len) * (thickness * 0.5f);

            int vBase = writtenVerts;
            mesh.SetNextVertex(new Vertex { position = new Vector3(x0 + nx, y0 + ny, Vertex.nearZ), tint = color });
            mesh.SetNextVertex(new Vertex { position = new Vector3(x1 + nx, y1 + ny, Vertex.nearZ), tint = color });
            mesh.SetNextVertex(new Vertex { position = new Vector3(x1 - nx, y1 - ny, Vertex.nearZ), tint = color });
            mesh.SetNextVertex(new Vertex { position = new Vector3(x0 - nx, y0 - ny, Vertex.nearZ), tint = color });
            writtenVerts += 4;

            mesh.SetNextIndex((ushort)vBase);
            mesh.SetNextIndex((ushort)(vBase + 1));
            mesh.SetNextIndex((ushort)(vBase + 2));
            mesh.SetNextIndex((ushort)vBase);
            mesh.SetNextIndex((ushort)(vBase + 2));
            mesh.SetNextIndex((ushort)(vBase + 3));
            writtenIndices += 6;
        }
    }
}
