using System;
using System.Threading;
using System.Threading.Tasks;

namespace NewsReader.Services.Abstractions
{
    public interface ITextToSpeechService
    {
        Task ReadText(string textToRead, CancellationToken  cancellationToken);
    }
}
