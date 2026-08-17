# Paintbox3000

## About

PaintBox3000 was developed as part of my vocational training in software development (IHK).
The project demonstrates the use of C#, WPF and object-oriented programming principles as well as 
connecting UI elements with the functionality of a simple drawing program.

## Features

- Draw lines, rectangles and ellipses
- Freehand drawing
- Adjustable brush size
- Round and square brush tips
- Stroke and fill color selection
- Adjustable canvas size
- Color history
- Undo and redo
- Open image files
- Drag and drop images onto the canvas
- Save images as PNG, JPEG or BMP

## Documentation 

For detailed instructions, see the [User Guide](docs/USER_GUIDE.md).

## Requirements

- Windows
- .NET 8
- Visual Studio 2022 or later
  
## Getting Started

1. Clone the repository.
2. Open `PaintBox3000.sln` in Visual Studio.
3. Build the solution.
4. Run the application.

## Technologies

- C#
- XAML
- WPF
- .NET 8
- Object-oriented design

## Architecture

The application separates UI orchestration from reusable functionality:

- `ColorCatalog` manages available colors and their properties.
- `ColorHistoryManager` manages recently used colors.
- `CanvasHistoryManager` provides undo/redo functionality.
- `ImageFileService` handles image loading and saving.
- `DrawableFactory` creates drawable objects based on the selected tool.

  ## Future Improvements
  
- Refactoring towards dependency injection
