namespace GerenteEstoque.View.Resources
{
  partial class CampoTexto
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
      this.TextBox = new System.Windows.Forms.TextBox();
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
      // TextBox
      // 
      this.TextBox.BackColor = System.Drawing.SystemColors.Window;
      this.TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.TextBox.Location = new System.Drawing.Point(200, 2);
      this.TextBox.Name = "TextBox";
      this.TextBox.Size = new System.Drawing.Size(395, 26);
      this.TextBox.TabIndex = 1;
      // 
      // CampoTexto
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.Color.Transparent;
      this.Controls.Add(this.TextBox);
      this.Controls.Add(this.Label);
      this.Name = "CampoTexto";
      this.Size = new System.Drawing.Size(600, 30);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label Label;
    private System.Windows.Forms.TextBox TextBox;
  }
}
