using System.Collections.Generic;
using SabberStoneAICompetition.BotPredatorMCTS.Reader;
using SabberStoneAICompetition.MCTSAlgorithm.MCTS;
using SabberStoneAICompetition.Score;
using SabberStoneCore.Model;
using SabberStoneCoreAi.Bigram;

namespace SabberStoneAICompetition.MCTSAlgorithm
{
	internal class PredatorMctsAgent : AbstractMctsAgent
	{
		private readonly PredictionParameters _predictionParameters;

		private readonly BigramMap _map;

		public PredatorMctsAgent(IScore scoring, MctsParameters mctsParameters, PredictionParameters predictionParameters)
			: base(scoring, mctsParameters)
		{

			_predictionParameters = predictionParameters;
			_map = BigramMapReader.ParseFile(_predictionParameters.File);
		}

		public static List<Card> ControlWarlock => new List<Card>(){
				//Dark Pact
				Cards.FromId ("LOOT_017"),
				Cards.FromId ("LOOT_017"),
				// Kobold Librarian
				Cards.FromId ("LOOT_014"),
				Cards.FromId ("LOOT_014"),
				// Defile
				Cards.FromId ("ICC_041"),
				Cards.FromId ("ICC_041"),
				// Stonehill Defender
				Cards.FromId ("UNG_072"),
				Cards.FromId ("UNG_072"),
				// Lesser Amethyst Spellstone
				Cards.FromId ("LOOT_043"),
				Cards.FromId ("LOOT_043"),
				// Hellfire
				Cards.FromId ("CS2_062"),
				Cards.FromId ("CS2_062"),
				// Possessed Lackey
				Cards.FromId ("LOOT_306"),
				Cards.FromId ("LOOT_306"),
				// Rin, the First Disciple
				Cards.FromId ("LOOT_415"),
				// Twisting Nether
				Cards.FromId ("EX1_312"),
				Cards.FromId ("EX1_312"),
				// Voidlord
				Cards.FromId ("LOOT_368"),
				Cards.FromId ("LOOT_368"),
				// Bloodreaver Gul'dan
				Cards.FromId ("ICC_831"),
				// Mistress of Mixtures
				Cards.FromId ("CFM_120"),
				Cards.FromId ("CFM_120"),
				// Doomsayer
				Cards.FromId ("NEW1_021"),
				Cards.FromId ("NEW1_021"),
				// N'Zoth, the Corruptor
				Cards.FromId ("OG_133"),
				// Siphon Soul
				Cards.FromId ("EX1_309"),
				// Skulking Geist
				Cards.FromId ("ICC_701"),
				//Mortal Coil
				Cards.FromId ("EX1_302"),
				// Gnomeferatu
				Cards.FromId ("ICC_407"),
				Cards.FromId ("ICC_407")
			};

		protected override AbstractMctsSimulator InitSimulator(int playerId, IScore scoring)
		{
			var ext = new MctsSimulatorExt(playerId, scoring,_mctsParameters,
				_predictionParameters, _map);
			ext.Watch = Watch;
			return ext;
		}
	}
}
