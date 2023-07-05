namespace HighlightMe.App
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            btnSave = new Button();
            pctBox = new PictureBox();
            btnLoadClipboard = new Button();
            btnRevertLast = new Button();
            btnClear = new Button();
            panel1 = new Panel();
            ((System.ComponentModel.ISupportInitialize)pctBox).BeginInit();
            SuspendLayout();
            // 
            // btnSave
            // 
            btnSave.Location = new Point(157, 12);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(145, 28);
            btnSave.TabIndex = 1;
            btnSave.Text = "Save Clipboard";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // pctBox
            // 
            pctBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pctBox.Location = new Point(12, 53);
            pctBox.Name = "pctBox";
            pctBox.Size = new Size(776, 385);
            pctBox.TabIndex = 2;
            pctBox.TabStop = false;
            // 
            // btnLoadClipboard
            // 
            btnLoadClipboard.Location = new Point(12, 12);
            btnLoadClipboard.Name = "btnLoadClipboard";
            btnLoadClipboard.Size = new Size(139, 28);
            btnLoadClipboard.TabIndex = 3;
            btnLoadClipboard.Text = "Load Clipboard";
            btnLoadClipboard.UseVisualStyleBackColor = true;
            btnLoadClipboard.Click += btnLoadClipboard_Click;
            // 
            // btnRevertLast
            // 
            btnRevertLast.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnRevertLast.Location = new Point(643, 12);
            btnRevertLast.Name = "btnRevertLast";
            btnRevertLast.Size = new Size(145, 28);
            btnRevertLast.TabIndex = 4;
            btnRevertLast.Text = "Revert last";
            btnRevertLast.UseVisualStyleBackColor = true;
            btnRevertLast.Click += btnRevertLast_Click;
            // 
            // btnClear
            // 
            btnClear.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnClear.Location = new Point(492, 12);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(145, 28);
            btnClear.TabIndex = 5;
            btnClear.Text = "Clear";
            btnClear.UseVisualStyleBackColor = true;
            btnClear.Click += btnClear_Click;
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panel1.BackColor = SystemColors.ControlText;
            panel1.Location = new Point(12, 46);
            panel1.Name = "panel1";
            panel1.Size = new Size(776, 1);
            panel1.TabIndex = 6;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(panel1);
            Controls.Add(btnClear);
            Controls.Add(btnRevertLast);
            Controls.Add(btnLoadClipboard);
            Controls.Add(pctBox);
            Controls.Add(btnSave);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "MainForm";
            Text = "Highlight Me - Made with ❤️ in Switzerland by Oliver Heil ";
            ((System.ComponentModel.ISupportInitialize)pctBox).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private Button btnSave;
        private PictureBox pctBox;
        private Button btnLoadClipboard;
        private Button btnRevertLast;
        private Button btnClear;
        private Panel panel1;
    }
}