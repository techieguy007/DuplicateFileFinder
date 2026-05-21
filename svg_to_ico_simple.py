#!/usr/bin/env python3
"""
Quick SVG to ICO converter for DuplicateFileFinder
Run this once to convert icon.svg to app.ico
"""

import os
import sys
from pathlib import Path

def convert_svg_to_ico_simple():
    """Convert SVG to ICO using PIL only (no cairosvg required)"""
    try:
        from PIL import Image, ImageDraw
    except ImportError:
        print("Error: Pillow not installed")
        print("Install with: pip install pillow")
        return False

    try:
        # Get paths
        project_dir = Path(__file__).parent / "DuplicateFileFinder"
        svg_path = project_dir / "icon.svg"
        ico_path = project_dir / "app.ico"

        if not svg_path.exists():
            print(f"❌ SVG file not found at {svg_path}")
            return False

        print(f"📦 Creating icon from SVG...")

        # Create icon programmatically matching the SVG design
        # This is a quick solution without needing cairosvg

        sizes = [(256, 256), (128, 128), (64, 64), (32, 32), (16, 16)]
        images = []

        for size in sizes:
            # Create image with blue gradient effect
            img = Image.new('RGBA', size, color=(0, 0, 0, 0))
            draw = ImageDraw.Draw(img)

            # Blue gradient background (approximate)
            # #2563EB to #4338CA
            for i in range(size[0]):
                r = int(37 + (67 - 37) * (i / size[0]))
                g = int(99 + (56 - 99) * (i / size[0]))
                b = int(235 + (202 - 235) * (i / size[0]))
                for j in range(size[1]):
                    img.putpixel((i, j), (r, g, b, 255))

            # Draw overlapping file rectangles (white and semi-transparent)
            rect_width = int(size[0] * 0.55)
            rect_height = int(size[1] * 0.7)

            # Background file (slightly offset)
            x1 = int(size[0] * 0.15)
            y1 = int(size[1] * 0.15)
            draw.rectangle([x1, y1, x1 + rect_width, y1 + rect_height], 
                          fill=(147, 197, 253, 200), outline=(211, 210, 239, 255), width=2)

            # Foreground file (white)
            x2 = int(size[0] * 0.3)
            y2 = int(size[1] * 0.25)
            draw.rectangle([x2, y2, x2 + rect_width, y2 + rect_height], 
                          fill=(255, 255, 255, 255), outline=(200, 200, 200, 255), width=2)

            # Magnifying glass (amber circle)
            glass_radius = int(size[0] * 0.15)
            glass_x = int(size[0] * 0.6)
            glass_y = int(size[1] * 0.6)
            draw.ellipse([glass_x - glass_radius, glass_y - glass_radius, 
                         glass_x + glass_radius, glass_y + glass_radius],
                        outline=(245, 158, 11, 255), width=max(1, int(size[0] // 16)))

            # Magnifying glass handle
            handle_end_x = int(glass_x + glass_radius * 1.2)
            handle_end_y = int(glass_y + glass_radius * 1.2)
            draw.line([(glass_x, glass_y), (handle_end_x, handle_end_y)],
                     fill=(245, 158, 11, 255), width=max(1, int(size[0] // 12)))

            images.append(img)

        # Save as ICO with multiple sizes
        images[0].save(ico_path, format='ICO', sizes=sizes, append_images=images[1:])

        print(f"✅ Successfully created {ico_path}")
        print(f"\n📋 Next steps:")
        print(f"1. Rebuild the Visual Studio project (Ctrl+Shift+B)")
        print(f"2. The icon should now appear in the application window")
        print(f"3. If not, try:")
        print(f"   - Right-click project → Properties")
        print(f"   - Application tab → Icon dropdown")
        print(f"   - Select app.ico")
        print(f"   - Rebuild")

        return True

    except Exception as e:
        print(f"❌ Error during conversion: {e}")
        import traceback
        traceback.print_exc()
        return False

if __name__ == "__main__":
    success = convert_svg_to_ico_simple()
    sys.exit(0 if success else 1)
