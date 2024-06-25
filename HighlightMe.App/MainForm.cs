using System.Drawing.Drawing2D;
using System.IO;

namespace HighlightMe.App;

public partial class MainForm : Form
{
    private const int blurSize = 8;
    private readonly List<Rectangle> highlightAreas = new();

    private Image? originalImage;
    private Image? bluredImage;
    private bool drag;
    private int dragStartx;
    private int dragStartY;
    private int dragCurX;
    private int dragCurY;
    private bool inFileMode;
    private string filePath = string.Empty;

    public MainForm()
    {
        InitializeComponent();
        KeyPreview = true;
        pctBox.MouseDown += PctBoxMouseDown;
        pctBox.MouseMove += PctBoxMouseMove;
        pctBox.MouseUp += PctBoxMouseUp;
        KeyDown += MainFormKeyDown;
        Clear();
    }

    private void SetImage(Image? original)
    {
        Clear();
        originalImage = original;

        if (originalImage is null)
            return;

        bluredImage = BlurHelper
            .FastGaussianBlur(new Bitmap(originalImage), blurSize);
        ApplyHighlight();
        btnSave.Enabled = true;
    }

    private void Clear()
    {
        originalImage = null;
        bluredImage = null;
        pctBox.Image = null;
        highlightAreas.Clear();
        btnSave.Enabled = false;
        btnRevertLast.Enabled = false;
    }

    private Image? CreateImage(bool hideSearcher = false)
    {
        if (bluredImage is null || originalImage is null)
            return null;

        Image clone;
        if (drag)
        {
            clone = (Image) originalImage.Clone();
        }
        else
        {
            clone = (Image) bluredImage.Clone();
        }

        using Graphics g = Graphics.FromImage(clone);
        using Pen pen = new Pen(Brushes.GreenYellow, 5);

        Rectangle currentDrag = default;
        if (drag)
            currentDrag = GenerateRectangleFromDimensions();

        foreach (var highlightArea in highlightAreas.Append(currentDrag))
        {
            if (highlightArea.Width <= 0 || highlightArea.Height <= 0)
                continue;

            var bitmap = new Bitmap(originalImage)
                .Clone(highlightArea, originalImage.PixelFormat);

            var brush = new TextureBrush(bitmap);
            brush.WrapMode = WrapMode.Tile;
            brush.TranslateTransform(highlightArea.X, highlightArea.Y);
            g.FillRectangle(brush, highlightArea);
            g.DrawRectangle(pen, highlightArea);
            brush.Dispose();
            bitmap.Dispose();
        }

        if (!hideSearcher && !drag)
        {
            const int searcherSize = 40;
            var searchWindow = new Rectangle(dragCurX - searcherSize / 2, dragCurY - searcherSize / 2, searcherSize, searcherSize);

            if (originalImage.Width < searchWindow.X + searchWindow.Width)
            {
                searchWindow.Width = searcherSize / 2;
                searchWindow.X = originalImage.Width - searchWindow.Width;
            }

            if (searchWindow.X < 0)
            {
                searchWindow.Width = searcherSize / 2;
                searchWindow.X = 0;
            }

            if (originalImage.Height < searchWindow.Y + searchWindow.Height)
            {
                searchWindow.Height = searcherSize / 2;
                searchWindow.Y = originalImage.Height - searchWindow.Height;
            }

            if (searchWindow.Y < 0)
            {
                searchWindow.Height = searcherSize / 2;
                searchWindow.Y = 0;
            }

            var bitmap = new Bitmap(originalImage)
                .Clone(searchWindow, originalImage.PixelFormat);
            var brush = new TextureBrush(bitmap);
            brush.WrapMode = WrapMode.Tile;
            brush.TranslateTransform(searchWindow.X, searchWindow.Y);
            g.FillRectangle(brush, searchWindow);
            brush.Dispose();
            bitmap.Dispose();
        }

        return clone;
    }

    private void ApplyHighlight()
    {
        if (bluredImage is null)
            return;

        pctBox.Image = CreateImage();
    }

    #region DrawNewHighlightArea

    private void PctBoxMouseUp(object? sender, MouseEventArgs e)
    {
        if (!drag)
            return;

        drag = false;

        var rectangle = GenerateRectangleFromDimensions();
        if (rectangle.Width <= 0 || rectangle.Height <= 0)
            return;

        highlightAreas.Add(rectangle);
        ApplyHighlight();
        btnRevertLast.Enabled = true;
    }

    private void PctBoxMouseMove(object? sender, MouseEventArgs e)
    {
        dragCurX = e.X;
        dragCurY = e.Y;
        ApplyHighlight();
    }

    private void PctBoxMouseDown(object? sender, MouseEventArgs e)
    {
        if (e.Button != MouseButtons.Left)
            return;

        drag = true;
        dragStartx = e.X;
        dragStartY = e.Y;
    }

    private Rectangle GenerateRectangleFromDimensions()
    {
        if (dragStartx < 0)
            dragStartx = 0;

        if (dragStartY < 0)
            dragStartY = 0;

        if (dragCurX < 0)
            dragCurX = 0;

        if (dragCurY < 0)
            dragCurY = 0;

        var width = originalImage?.Size.Width ?? 0;
        if (dragStartx > width)
            dragStartx = width;

        if (dragCurX > width)
            dragCurX = width;

        var height = originalImage?.Size.Height ?? 0;

        if (dragStartY > height)
            dragStartY = height;

        if (dragCurY > height)
            dragCurY = height;

        var rectangle = new Rectangle(Math.Min(dragStartx, dragCurX),
            Math.Min(dragStartY, dragCurY),
            Math.Abs(dragStartx - dragCurX),
            Math.Abs(dragStartY - dragCurY));
        return rectangle;
    }

    #endregion

    #region ButtonHandler
    
    private void btnLoadClipboard_Click(object? sender, EventArgs e)
    {
        Clear();
        if (inFileMode)
        {
            using var openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.png;*.jpeg;*.jpg;*.bmp)|*.png;*.jpeg;*.jpg;*.bmp|All files (*.*)|*.*",
                Title = "Select an image file"
            };

            if (openFileDialog.ShowDialog() != DialogResult.OK)
                return;
            
            var image = Image.FromFile(openFileDialog.FileName);
            filePath = openFileDialog.FileName;
            SetImage(image);
        }
        else
        {
            if (!Clipboard.ContainsImage())
                return;

            var image = Clipboard.GetImage();
            SetImage(image);
        }
    }

    private void btnSave_Click(object? sender, EventArgs e)
    {
        var image = CreateImage(true);
        if (image is null)
            return;

        if (inFileMode)
        {
            image.Save(filePath);
        }
        else
        {
            Clipboard.SetImage(image);
        }
    }

    private void btnRevertLast_Click(object? sender, EventArgs e)
    {
        if (!highlightAreas.Any())
            return;

        highlightAreas.Remove(highlightAreas.LastOrDefault());
        if (!highlightAreas.Any())
            btnRevertLast.Enabled = true;
        ApplyHighlight();
    }

    private void btnClear_Click(object? sender, EventArgs e)
    {
        Clear();
    }

    private void MainFormKeyDown(object? sender, KeyEventArgs e)
    {
        if (e is { Control: true, KeyCode: Keys.S })
            btnSave_Click(sender, e);

        if (e is { Control: true, KeyCode: Keys.Z })
            btnRevertLast_Click(sender, e);
    }
    
    private void btnToggleInput_Click(object sender, EventArgs e)
    {
        inFileMode = !inFileMode;
        SetButtonText();
    }

    private void SetButtonText()
    {
        if (inFileMode)
        {
            btnLoadClipboard.Text = "Load File";
            btnSave.Text = "Save File";
            btnToggleInput.Text = "Have in Clipboard?";
        }
        else
        {
            btnLoadClipboard.Text = "Load Clipboard";
            btnSave.Text = "Save Clipboard";
            btnToggleInput.Text = "Have a File?";
        }
    }

    #endregion
}