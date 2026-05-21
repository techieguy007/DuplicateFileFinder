#!/usr/bin/env python3
"""
Convert SVG icon to ICO format for DuplicateFileFinder application

Usage:
    python svg_to_ico.py

Requirements:
    pip install pillow cairosvg
"""

import os
import sys
from pathlib import Path

def convert_svg_to_ico():
    """Convert icon.svg to app.ico for use in Windows Forms application"""

    try:
        from PIL import Image
        import cairosvg
        import io
    except ImportError:
        print("Error: Required libraries not installed.")
        print("Install with: pip install pillow cairosvg")
        return False

    try:
        # Get the script directory
        script_dir = Path(__file__).parent
        svg_path = script_dir / "DuplicateFileFinder" / "icon.svg"
        ico_path = script_dir / "DuplicateFileFinder" / "app.ico"

        if not svg_path.exists():
            print(f"Error: SVG file not found at {svg_path}")
            return False

        print(f"Converting {svg_path} to ICO...")

        # Convert SVG to PNG in memory
        png_data = io.BytesIO()
        cairosvg.svg2png(url=str(svg_path), write_to=png_data, output_width=256, output_height=256)
        png_data.seek(0)

        # Open PNG and convert to ICO
        image = Image.open(png_data)

        # Create multiple sizes for ICO format (256x256, 128x128, 64x64, 32x32, 16x16)
        sizes = [(256, 256), (128, 128), (64, 64), (32, 32), (16, 16)]
        images = []

        for size in sizes:
            img = image.convert("RGBA").resize(size, Image.Resampling.LANCZOS)
            images.append(img)

        # Save as ICO
        images[0].save(ico_path, format='ICO', sizes=sizes, append_images=images[1:])

        print(f"✅ Successfully created {ico_path}")
        print("\nNext steps:")
        print("1. In Visual Studio, open DuplicateFileFinder project properties")
        print("2. Go to Application tab")
        print("3. Click 'Icon and manifest' -> 'Icon'")
        print("4. Select the newly created app.ico file")
        print("5. Rebuild the project")

        return True

    except Exception as e:
        print(f"Error during conversion: {e}")
        return False

if __name__ == "__main__":
    success = convert_svg_to_ico()
    sys.exit(0 if success else 1)
