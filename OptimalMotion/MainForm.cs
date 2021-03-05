using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using OptimalMoving.Domain;
using OptimalMoving.Enums;

namespace OptimalMoving
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            DoubleBuffered = true;

            InitializeComponent();

            table = GetTable();

            InitButtons();
            InitClockLabel();

            topLayout = GetTopLayout();
            mainLayout = GetMainLayout();
            Controls.Add(mainLayout);

            

            model = new Model(1, 1, table, clock);
        }

        private readonly IModel model;
        private readonly ITable table;
        private readonly TableLayoutPanel mainLayout = new TableLayoutPanel();
        private readonly TableLayoutPanel topLayout = new TableLayoutPanel();
        private readonly DataGridView tableDataGridView = new DataGridView();
        private readonly Label clock = new Label();

        public Button StartButton { get; private set; }
        public Button StopButton { get; private set; }
        public Button PauseButton { get; private set; }


        private ITable GetTable()
        {
            tableDataGridView.Dock = DockStyle.Fill;

            tableDataGridView.Font = new Font("Roboto", 14f, FontStyle.Bold, GraphicsUnit.Pixel);
            tableDataGridView.DefaultCellStyle.Font = new Font("Roboto", 14F, GraphicsUnit.Pixel);

            return new Table(tableDataGridView);
        }

        private void TableDataGridViewOnCellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            for (var i = 0; i < tableDataGridView.Columns.Count - 1; i++)
            {
                tableDataGridView.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
        }

        private void TableDataGridView_DataSourceChanged(object sender, EventArgs e)
        {
            for (var i = 0; i < tableDataGridView.Columns.Count - 1; i++)
            {
                tableDataGridView.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
        }

        private TableLayoutPanel GetMainLayout()
        {
            mainLayout.ColumnCount = 1;
            mainLayout.ColumnStyles.Add(new ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            mainLayout.Controls.Add(topLayout, 0, 0);
            mainLayout.Controls.Add(tableDataGridView, 0, 1);
            //mainLayout.Location = new System.Drawing.Point(288, 67);
            mainLayout.Name = "mainLayout";
            mainLayout.RowCount = 2;
            mainLayout.RowStyles.Add(new RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            mainLayout.RowStyles.Add(new RowStyle(System.Windows.Forms.SizeType.Percent, 75F));
            mainLayout.Dock = DockStyle.Fill;

            return mainLayout;
        }

        private TableLayoutPanel GetTopLayout()
        {
            topLayout.ColumnCount = 4;
            topLayout.ColumnStyles.Add(new ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            topLayout.ColumnStyles.Add(new ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            topLayout.ColumnStyles.Add(new ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            topLayout.ColumnStyles.Add(new ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            topLayout.Controls.Add(StartButton, 0, 0);
            topLayout.Controls.Add(StopButton, 1, 0);
            topLayout.Controls.Add(PauseButton, 2, 0);
            topLayout.Controls.Add(clock, 3, 0);
            //topLayout.Location = new System.Drawing.Point(288, 67);
            topLayout.Name = "topLayout";
            topLayout.RowCount = 1;
            topLayout.RowStyles.Add(new RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            topLayout.Dock = DockStyle.Fill;

            return topLayout;
        }

        private void InitClockLabel()
        {
            clock.TextAlign = ContentAlignment.MiddleCenter;
            clock.Dock = DockStyle.Fill;
            clock.Text = string.Format("{0:00}:{1:00}:{2:00}", 0, 0, 0);
        }

        private void InitButtons()
        {
            InitStartButton();
            InitStopButton();
            InitPauseButton();
        }

        private void InitStartButton()
        {
            StartButton = new Button();
            StartButton.Text = "Start";
            StartButton.Click += StartButtonOnClick;
            Controls.Add(StartButton);
        }

        private void InitStopButton()
        {
            StopButton = new Button();
            StopButton.Text = "Stop";
            StopButton.Click += StopButtonOnClick;
            Controls.Add(StopButton);
        }

        private void InitPauseButton()
        {
            PauseButton = new Button();
            PauseButton.Text = "Pause";
            PauseButton.Click += PauseButtonOnClick;
            Controls.Add(PauseButton);
        }

        private void StartButtonOnClick(object sender, EventArgs e)
        {
            model.ChangeStage(ModelStages.Started);
        }

        private void StopButtonOnClick(object sender, EventArgs e)
        {
            model.ChangeStage(ModelStages.Preparing);
        }

        private void PauseButtonOnClick(object sender, EventArgs e)
        {
            model.ChangeStage(ModelStages.Paused);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Space) 
                Close();
        }
    }
}
