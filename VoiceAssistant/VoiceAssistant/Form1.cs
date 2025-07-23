using System;
using System.Diagnostics;
using System.Drawing;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Windows.Forms;

namespace VoiceAssistant
{
    public class Form1 : Form
    {
        private Label headingLabel;
        private TextBox outputBox;
        private Button startButton;
        private SpeechRecognitionEngine recognizer;
        private SpeechSynthesizer synthesizer;

        public Form1()
        {
            InitializeComponent();
            SetupVoiceAssistant();
        }

        private void InitializeComponent()
        {
            this.Text = "Voice Assistant (Dark Mode)";
            this.ClientSize = new Size(500, 400);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(30, 30, 30);

            headingLabel = new Label
            {
                Text = "🗣 Voice Assistant",
                Font = new Font("Calibri", 18F, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 60
            };

            outputBox = new TextBox
            {
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical,
                Font = new Font("Consolas", 11F),
                Size = new Size(460, 240),
                Location = new Point(20, 80),
                BackColor = Color.FromArgb(45, 45, 48),
                ForeColor = Color.White
            };

            startButton = new Button
            {
                Text = "Start Listening",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                Size = new Size(200, 40),
                Location = new Point(150, 330),
                BackColor = Color.MediumPurple,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            startButton.FlatAppearance.BorderSize = 0;
            startButton.Click += StartButton_Click;

            this.Controls.Add(headingLabel);
            this.Controls.Add(outputBox);
            this.Controls.Add(startButton);
        }

        private void SetupVoiceAssistant()
        {
            synthesizer = new SpeechSynthesizer();
            recognizer = new SpeechRecognitionEngine();

            Choices commands = new Choices(new string[] {"hello", "what is your name","search google", "exit"});

            GrammarBuilder gb = new GrammarBuilder(commands);
            Grammar grammar = new Grammar(gb);

            recognizer.LoadGrammar(grammar);
            recognizer.SetInputToDefaultAudioDevice();
            recognizer.SpeechRecognized += Recognizer_SpeechRecognized;
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            synthesizer.SpeakAsync("Listening activated.");
            recognizer.RecognizeAsync(RecognizeMode.Multiple);
            outputBox.AppendText("🎤 Listening...\r\n");
            startButton.Enabled = false;
            startButton.Text = "Listening...";
            startButton.BackColor = Color.Gray;
        }

        private void Recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string command = e.Result.Text;
            outputBox.AppendText("You said: " + command + Environment.NewLine);

            switch (command)
            {
                case "hello":
                    synthesizer.SpeakAsync("Hi there!");
                    break;
                case "what is your name":
                    synthesizer.SpeakAsync("I am your open-source assistant.");
                    break;
                case "search google":
                    synthesizer.Speak("What do you want to search?");
                    recognizer.RecognizeAsyncCancel();
                    string query = Microsoft.VisualBasic.Interaction.InputBox("Enter your search:");
                    if (!string.IsNullOrWhiteSpace(query))
                    {
                        Process.Start("https://www.google.com/search?q=" + Uri.EscapeDataString(query));
                    }
                    recognizer.RecognizeAsync(RecognizeMode.Multiple);
                    break;
                case "exit":
                    synthesizer.Speak("Goodbye!");
                    Application.Exit();
                    break;
                default:
                    synthesizer.SpeakAsync("Sorry, I didn't understand.");
                    break;
            }
        }
    }
}