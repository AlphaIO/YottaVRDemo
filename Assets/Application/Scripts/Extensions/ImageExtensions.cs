using UnityEngine;
using UnityEngine.UI;

public static class GraphicExtensions
{
	public static void SetAlpha(this Graphic image, float alpha)
	{
        if (image == null)
            return;

        var c = image.color;
		c.a = alpha;
		image.color = c;
	}

	public static void SetColor(this Graphic image, Color color)
	{
        if (image == null)
            return;
		var c = image.color;
		c.r = color.r;
		c.g = color.g;
		c.b = color.b;
		image.color = c;
	}
}
