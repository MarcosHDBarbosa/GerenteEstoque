namespace GerenteEstoque.View.Resources
{
  partial class CampoSelect
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
      this.Label = new System.Windows.Forms.Label();
      this.ComboBox = new System.Windows.Forms.ComboBox();
      this.Button = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // Label
      // 
      this.Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.Label.Location = new System.Drawing.Point(5, 5);
      this.Label.Name = "Label";
      this.Label.Size = new System.Drawing.Size(195, 20);
      this.Label.TabIndex = 0;
      this.Label.Text = "NomeCampo";
      this.Label.TextAlign = System.Drawing.ContentAlignment.TopRight;
      // 
      // ComboBox
      // 
      this.ComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
      this.ComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
      this.ComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ComboBox.FormattingEnabled = true;
      this.ComboBox.Location = new System.Drawing.Point(200, 1);
      this.ComboBox.Name = "ComboBox";
      this.ComboBox.Size = new System.Drawing.Size(250, 28);
      this.ComboBox.TabIndex = 1;
      // 
      // Button
      // 
      this.Button.BackColor = System.Drawing.Color.Transparent;
      this.Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.Button.Location = new System.Drawing.Point(495, 1);
      this.Button.Name = "Button";
      this.Button.Size = new System.Drawing.Size(100, 28);
      this.Button.TabIndex = 2;
      this.Button.Text = "Criar";
      this.Button.UseVisualStyleBackColor = false;
      this.Button.Click += new System.EventHandler(this.Button_Click);
      // 
      // CampoSelect
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.Color.Transparent;
      this.Controls.Add(this.Button);
      this.Controls.Add(this.ComboBox);
      this.Controls.Add(this.Label);
      this.Name = "CampoSelect";
      this.Size = new System.Drawing.Size(600, 30);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Label Label;
    private System.Windows.Forms.ComboBox ComboBox;
    private System.Windows.Forms.Button Button;
  }
}
