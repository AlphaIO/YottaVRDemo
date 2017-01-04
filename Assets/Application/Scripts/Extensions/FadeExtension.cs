using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public static class FadeExtension
{
    public static void ColorThemAll(this Component c, Color color, float duration)
    {
        ColorThemAll(c.gameObject, color, duration);
    }

    public static void ColorThemAll(this GameObject go, Color color, float duration)
    {
        foreach (var r in go.GetComponentsInChildren<Renderer>(true))
        {
            if (r.material == null)
            {
                continue;
            }

            if (r.material.HasProperty("_TintColor"))
            {
                if (duration < float.Epsilon)
                {
                    var c = r.material.GetColor("_TintColor");
                    c.r = color.r;
                    c.g = color.g;
                    c.b = color.b;
                    r.material.SetColor("_TintColor", c);
                }
                else
                {
                    var c = r.material.GetColor("_TintColor");
                    c.r = color.r;
                    c.g = color.g;
                    c.b = color.b;
                    r.material.DOColor(c, "_TintColor", duration);
                }
            }
            else
            {
                if (duration < float.Epsilon)
                {
                    var c = r.material.color;
                    c.r = color.r;
                    c.g = color.g;
                    c.b = color.b;
                    r.material.color = c;
                }
                else
                {
                    var c = r.material.color;
                    c.r = color.r;
                    c.g = color.g;
                    c.b = color.b;
                    r.material.DOColor(c, duration);
                }
            }
        }

        foreach (var g in go.GetComponentsInChildren<Graphic>(true))
        {
            if (duration < float.Epsilon)
            {
                g.color = color;
            }
            else
            {
                g.DOColor(color, duration);
            }
        }
    }

    public static void FadeThemAll(this Component c, float alpha, float duration)
    {
        if (c != null && c.gameObject)
            FadeThemAll(c.gameObject, alpha, duration);
    }

    public static void FadeThemAll(this GameObject go, float alpha, float duration)
    {
        foreach (var r in go.GetComponentsInChildren<Renderer>(true))
        {
            if (r.material == null)
            {
                continue;
            }

            if (r.material.HasProperty("_TintColor"))
            {
                if (duration < float.Epsilon)
                {
                    var c = r.material.GetColor("_TintColor");
                    c.a = alpha;
                    r.material.SetColor("_TintColor", c);
                }
                else
                {
                    r.material.DOFade(alpha, "_TintColor", duration);
                }
            }
            else
            {
                if (duration < float.Epsilon)
                {
                    var c = r.material.color;
                    c.a = alpha;
                    r.material.color = c;
                }
                else
                {
                    r.material.DOFade(alpha, duration);
                }
            }
        }

        foreach (var s in go.GetComponentsInChildren<AudioSource>(true))
        {
            if (duration < float.Epsilon)
            {
                s.volume = alpha;
            }
            else
            {
                s.DOFade(alpha, duration);
            }
        }

        foreach (var g in go.GetComponentsInChildren<Graphic>(true))
        {
            if (duration < float.Epsilon)
            {
                g.SetAlpha(alpha);
            }
            else
            {
                g.DOFade(alpha, duration);
            }
        }
    }

    public static void FadeThis(this GameObject go, float alpha, float duration)
    {
        var r = go.GetComponent<Renderer>();


        if (r != null && r.material)
        {
            if (r.material.HasProperty("_TintColor"))
            {
                if (duration < float.Epsilon)
                {
                    var c = r.material.GetColor("_TintColor");
                    c.a = alpha;
                    r.material.SetColor("_TintColor", c);
                }
                else
                {
                    r.material.DOFade(alpha, "_TintColor", duration);
                }
            }
            else
            {
                if (duration < float.Epsilon)
                {
                    var c = r.material.color;
                    c.a = alpha;
                    r.material.color = c;
                }
                else
                {
                    r.material.DOFade(alpha, duration);
                }
            }
        }

        var s = go.GetComponent<AudioSource>();
        if (s != null)
        {
            if (duration < float.Epsilon)
            {
                s.volume = alpha;
            }
            else
            {
                s.DOFade(alpha, duration);
            }
        }
        var g = go.GetComponent<Graphic>();
        if (g != null) { 
            if (duration < float.Epsilon)
            {
                g.SetAlpha(alpha);
            }
            else
            {
                g.DOFade(alpha, duration);
            }
    }

    }

    public static void FadeThis(this Component c, float alpha, float duration)
    {
        if (c != null && c.gameObject)
            FadeThis(c.gameObject, alpha, duration);
    }

    public static float GetAlpha(this Component c) {
        if (c.gameObject)
            return GetAlpha(c.gameObject);
        else
            return 1;
    }
    public static float GetAlpha(this GameObject go)
    {
        var r = go.GetComponent<Renderer>();
        var g = go.GetComponent<Graphic>();

        if (r != null && r.material)
        {

            return r.material.color.a;


        }

        else if (g != null)
        {
            return g.color.a;
        }
        else
            return 1;

    }

}
