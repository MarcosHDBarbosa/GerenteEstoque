namespace GerenteEstoque.View.Resources
{
  partial class CampoSelectVarios
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
      this.Button_Criar = new System.Windows.Forms.Button();
      this.Button_Adicionar = new System.Windows.Forms.Button();
      this.FlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
      this.FlowLayoutPanel.SuspendLayout();
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
      // Button_Criar
      // 
      this.Button_Criar.BackColor = System.Drawing.Color.Transparent;
      this.Button_Criar.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.Button_Criar.Location = new System.Drawing.Point(495, 1);
      this.Button_Criar.Name = "Button_Criar";
      this.Button_Criar.Size = new System.Drawing.Size(100, 28);
      this.Button_Criar.TabIndex = 2;
      this.Button_Criar.Text = "Criar";
      this.Button_Criar.UseVisualStyleBackColor = false;
      this.Button_Criar.Click += new System.EventHandler(this.Button_Click);
      // 
      // Button_Adicionar
      // 
      this.Button_Adicionar.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.Button_Adicionar.Location = new System.Drawing.Point(3, 3);
      this.Button_Adicionar.Name = "Button_Adicionar";
      this.Button_Adicionar.Size = new System.Drawing.Size(100, 28);
      this.Button_Adicionar.TabIndex = 0;
      this.Button_Adicionar.Text = "Adicionar";
      this.Button_Adicionar.UseVisualStyleBackColor = true;
      this.Button_Adicionar.Click += new System.EventHandler(this.Button_Adicionar_Click);
      // 
      // FlowLayoutPanel
      // 
      this.FlowLayoutPanel.Controls.Add(this.Button_Adicionar);
      this.FlowLayoutPanel.Location = new System.Drawing.Point(200, 0);
      this.FlowLayoutPanel.Name = "FlowLayoutPanel";
      this.FlowLayoutPanel.Size = new System.Drawing.Size(286, 33);
      this.FlowLayoutPanel.TabIndex = 1;
      // 
      // CampoSelectVarios
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.AutoSize = true;
      this.BackColor = System.Drawing.Color.Transparent;
      this.Controls.Add(this.FlowLayoutPanel);
      this.Controls.Add(this.Button_Criar);
      this.Controls.Add(this.Label);
      this.Name = "CampoSelectVarios";
      this.Size = new System.Drawing.Size(600, 36);
      this.FlowLayoutPanel.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Label Label;
    private System.Windows.Forms.Button Button_Criar;
    private System.Windows.Forms.Button Button_Adicionar;
    private System.Windows.Forms.FlowLayoutPanel FlowLayoutPanel;
  }
}
