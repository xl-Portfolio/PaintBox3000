# User Guide

## Overview

PaintBox3000 provides a straightforward interface for creating simple drawings and editing imported images. 
The toolbar provides quick access to the main drawing and history functions, while the sidebar provides 
control over colors and brush settings. Drawings can be saved as image files and existing images can be imported 
into your canvas. For installation and project information, see the README.

## Application Interface

![screenshot](Paintbox3000_screenshot4.png)

'Menu Bar (1)' – Provides operations such as opening and saving files, and closing the application.
'Toolbar' – Contains 'undo, redo and clear buttons (2)', 'drawing tools (3)', and the 'color history (4)'.
'Canvas (5)' – The main area where drawings are created.
'Sidebar (6)' – Contains settings for the currently selected tool.
'Status Bar (7)' – Displays the currently selected tool, brush size, stroke color, and fill color.

## Drawing Tools

PaintBox3000 provides four drawing tools:

- 'Line' – draws a straight line between two points.
- 'Ellipse' – draws an ellipse (or circle, if drawn with equal width and height).
- 'Rectangle' – draws a rectangle (or square, if drawn with equal width and height).
- 'Freehand' – draws a freeform line that follows the mouse cursor while dragging.

To draw a shape, select a tool from the toolbar, then click and hold on the canvas at the starting point and drag to the desired end point. For the Freehand tool, simply drag the cursor along the path[...]

## Tool Settings

Selecting a drawing tool opens the sidebar with the settings relevant to that tool:

- 'Stroke Color' – the outline color, available for all tools.
- 'Fill Color' – the interior color, available for the Ellipse and Rectangle tools only.
- 'Brush Size' – the thickness of the stroke, adjustable via a slider.
- 'Brush Tip' – round or square line ends, available for the Line and Freehand tools only.

The sidebar can be closed manually and reopens automatically the next time a drawing tool is selected.

## Color History

The toolbar's color history dropdowns keep track of the most recently used stroke and fill colors (up to ten each), letting you reapply a previous color without returning to the sidebar.

## Undo, Redo and Clear

- 'Undo' – removes the most recently drawn shape or imported image from the canvas.
- 'Redo' – restores the element that was last removed with Undo.
- 'Clear' – removes all elements from the canvas at once. Cleared elements cannot be restored with Redo.

## Working with Images

- 'Open' (Menu Bar) – opens a file dialog to import an image (JPG, PNG, BMP, GIF, or TIFF) onto the canvas.
- Drag & drop – image files can also be imported by dragging them from the file explorer directly onto the canvas.
- 'Save' (Menu Bar) – exports the current canvas as an image file in PNG, JPEG, or BMP format.

## Closing the Application

- Click the X-button in the top right corner of the window or select 'Close' in the Menu Bar to exit PaintBox3000.
