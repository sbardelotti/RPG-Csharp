
namespace RPG_GAME
{
    partial class f_rpg_game
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lb_hitPoints = new System.Windows.Forms.Label();
            this.lb_gold = new System.Windows.Forms.Label();
            this.lb_experience = new System.Windows.Forms.Label();
            this.lb_level = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Hit Points:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Gold:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Experience:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(18, 100);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(36, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Level:";
            // 
            // lb_hitPoints
            // 
            this.lb_hitPoints.AutoSize = true;
            this.lb_hitPoints.Location = new System.Drawing.Point(110, 19);
            this.lb_hitPoints.Name = "lb_hitPoints";
            this.lb_hitPoints.Size = new System.Drawing.Size(11, 13);
            this.lb_hitPoints.TabIndex = 4;
            this.lb_hitPoints.Text = "-";
            // 
            // lb_gold
            // 
            this.lb_gold.AutoSize = true;
            this.lb_gold.Location = new System.Drawing.Point(110, 45);
            this.lb_gold.Name = "lb_gold";
            this.lb_gold.Size = new System.Drawing.Size(11, 13);
            this.lb_gold.TabIndex = 5;
            this.lb_gold.Text = "-";
            // 
            // lb_experience
            // 
            this.lb_experience.AutoSize = true;
            this.lb_experience.Location = new System.Drawing.Point(110, 73);
            this.lb_experience.Name = "lb_experience";
            this.lb_experience.Size = new System.Drawing.Size(11, 13);
            this.lb_experience.TabIndex = 6;
            this.lb_experience.Text = "-";
            // 
            // lb_level
            // 
            this.lb_level.AutoSize = true;
            this.lb_level.Location = new System.Drawing.Point(110, 99);
            this.lb_level.Name = "lb_level";
            this.lb_level.Size = new System.Drawing.Size(11, 13);
            this.lb_level.TabIndex = 7;
            this.lb_level.Text = "-";
            // 
            // f_rpg_game
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(729, 665);
            this.Controls.Add(this.lb_level);
            this.Controls.Add(this.lb_experience);
            this.Controls.Add(this.lb_gold);
            this.Controls.Add(this.lb_hitPoints);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "f_rpg_game";
            this.Text = "Rpg Simple Game";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lb_hitPoints;
        private System.Windows.Forms.Label lb_gold;
        private System.Windows.Forms.Label lb_experience;
        private System.Windows.Forms.Label lb_level;
    }
}

