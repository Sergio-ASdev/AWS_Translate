using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Translate;
using Amazon.Translate.Model;
using Microsoft.Extensions.Configuration;

namespace translate_terminologies {
	class Program {
		// const string EnglishText = @"Amazon Translate";
		
		static void Main(string[] args) {
			Console.Write("Texto en Inglés a traducir: ");
			string EnglishText = Console.ReadLine();
			EnglishText = EnglishText.Trim();
			var awsOptions = BuildAwsOptions();
			var service = new TranslateService(awsOptions.CreateServiceClient<IAmazonTranslate>());

			// simple translation
			var translateTask = service.TranslateText(EnglishText, "en", "es");
			translateTask.Wait();
			var result = translateTask.Result;
			var translatedText = result.TranslatedText;
			//Console.WriteLine("Source: {0}", EnglishText);
			Console.WriteLine();
			Console.WriteLine("Translation: {0}", translatedText);
			Console.WriteLine();

		}

		private static AWSOptions BuildAwsOptions() {
			var builder = new ConfigurationBuilder()
				.SetBasePath(Environment.CurrentDirectory)
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				.Build();
			return builder.GetAWSOptions();
		}
	}

	public class TranslateService {
		private IAmazonTranslate translate;
		public TranslateService(IAmazonTranslate translate) {
			this.translate = translate;
		}

		public async Task<TranslateTextResponse> TranslateText(string text, string sourceLanguage, string targetLanguage) {
			return await this.TranslateText(text, sourceLanguage, targetLanguage, null);
		}

		public async Task<TranslateTextResponse> TranslateText(string text, string sourceLanguage, string targetLanguage, List<string> terminologies) {
			var request = new TranslateTextRequest {
				SourceLanguageCode = sourceLanguage,
				TargetLanguageCode = targetLanguage,
				TerminologyNames = terminologies,
				Text = text
			};

			return await this.translate.TranslateTextAsync(request);
		}
	}

}
