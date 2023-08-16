using System.Globalization;
using System.Speech.Synthesis;
using NewsReader.Services.Abstractions;

namespace NewsReader.Services.Implementation
{
    public class TextToSpeechService : ITextToSpeechService
    {
        public Task ReadText(string textToRead, CancellationToken cancellationToken)
        {
            SpeechSynthesizer synthesizer = new SpeechSynthesizer();
            synthesizer.Volume = 100;  // 0...100
            synthesizer.Rate = -2;     // -10...10

            synthesizer.Speak(textToRead);
            
            /**
            var synthesizer = new SpeechSynthesizer();
            synthesizer.SetOutputToDefaultAudioDevice();
            
            var builder = new PromptBuilder();
            builder.StartVoice(new CultureInfo("en-US"));
            builder.AppendText("All we need to do is to keep talking.");
            builder.EndVoice();
            
            synthesizer.Speak(builder);
            */
            throw new NotImplementedException();
        }
    }
}