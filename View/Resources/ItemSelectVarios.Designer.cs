namespace GerenteEstoque.View.Resources
{
  partial class ItemSelectVarios
  {
    /// <summary> 
    /// Variável de designer necessária.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary> 
    /// Limpar os recursos que estão sendo usados.
    /// </summary>
    /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Código gerado pelo Designer de Componentes

    /// <summary> 
    /// Método necessário para suporte ao Designer - não modifique 
    /// o conteúdo deste método com o editor de código.
    /// </summary>
    private void InitializeComponent()
    {
      this.ComboBox = new System.Windows.Forms.ComboBox();
      this.Button = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // ComboBox
      // 
      this.ComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
      this.ComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
      this.ComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ComboBox.FormattingEnabled = true;
      this.ComboBox.Location = new System.Drawing.Point(0, 1);
      this.ComboBox.Name = "ComboBox";
      this.ComboBox.Size = new System.Drawing.Size(250, 28);
      this.ComboBox.TabIndex = 2;
      // 
      // Button
      // 
      this.Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.Button.Location = new System.Drawing.Point(250, 0);
      this.Button.Name = "Button";
      this.Button.Size = new System.Drawing.Size(30, 30);
      this.Button.TabIndex = 3;
      this.Button.Text = "X";
      this.Button.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.Button.UseVisualStyleBackColor = true;
      this.Button.Click += new System.EventHandler(this.Button_Click);
      // 
      // ItemSelectVarios
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.Button);
      this.Controls.Add(this.ComboBox);
      this.Name = "ItemSelectVarios";
      this.Size = new System.Drawing.Size(280, 30);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.ComboBox ComboBox;
    private System.Windows.Forms.Button Button;
  }
}
