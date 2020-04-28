using System;
using System.Collections.Generic;
using SabberStoneAICompetition.AIAgents;
using SabberStoneAICompetition.BotPredatorMCTS;
using SabberStoneAICompetition.MCTSAlgorithm.MCTS;
using SabberStoneAICompetition.Score;
using SabberStoneBasicAI.AIAgents;
using SabberStoneCore.Enums;
using SabberStoneCore.Model;

namespace SabberStoneAICompetition.MCTSAlgorithm
{
	/// <summary>
	/// Factory singleton class for creating abstract agents.
	/// </summary>
    class AgentFactory
    {
		public List<Card> deck = null;
		public CardClass hero = CardClass.INVALID;

		/// <summary>
		/// The type of heartstone agents.
		/// </summary>
		public enum AgentType
		{
			PredatorMCTS,
		}

		/// <summary>
		/// The singleton instance.
		/// </summary>
		private static AgentFactory _instance;

		/// <summary>
		/// The factory instance.
		/// </summary>
		public static AgentFactory Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new AgentFactory();
				}
				return _instance;
			}
		}

		/// <summary>
		/// The private default constructor.
		/// </summary>
		public AgentFactory()
		{
			deck = ControlWarlock;
			hero = CardClass.WARLOCK;
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

	/// <summary>
	/// Returns a instance of a Hearthstone agent based on the profited card class and agent type.
	/// The card class of the hero is key for configuring the agent correctly, especially the predator MCTS.
	/// </summary>
	/// <param name="cardClass">the card class of the agent's hero</param>
	/// <param name="type">the type of agent</param>
	/// <returns></returns>
	public AbstractAgent GetAgent(CardClass cardClass,  AgentType type)
		{
			double simulationTime = 25000 ;
			IScore scoring = new WeightedScore();

			AbstractAgent agent = new RandomAgent();
			switch (type)
			{
				case AgentType.PredatorMCTS:
					switch (cardClass)
					{
						// the default decks
						case CardClass.WARRIOR:
							Console.WriteLine("Aggro Deck");
							agent = new PredatorMctsAgent(scoring,
								new MctsParameters
								{
									SimulationTime = simulationTime,
									AggregationTime = 100,
									RolloutDepth = 5,
									UCTConstant = 9000
								},
								new PredictionParameters
								{
									File = Environment.CurrentDirectory + @"\src\Bigramms\bigramm_1-2017-12-2016.json.gz",
									CardCount = 10,
									StepWidth = 2,
									DeckCount = 1,
									SetCount = 3,
									LeafCount = 5,
									SimulationDepth = 1,
								});
							break;
						case CardClass.SHAMAN:
							agent = new PredatorMctsAgent(scoring,
								new MctsParameters
								{
									SimulationTime = simulationTime,
									AggregationTime = 100,
									RolloutDepth = 5,
									UCTConstant = 9000
								},
								new PredictionParameters
								{
									File = Environment.CurrentDirectory + @"\src\Bigramms\bigramm_1-2017-12-2016.json.gz",
									CardCount = 10,
									StepWidth = 2,
									DeckCount = 1,
									SetCount = 3,
									LeafCount = 5,
									SimulationDepth = 3,
								});
							break;
						case CardClass.MAGE:
							agent = new PredatorMctsAgent(scoring,
								new MctsParameters
								{
									SimulationTime = simulationTime,
									AggregationTime = 100,
									RolloutDepth = 5,
									UCTConstant = 9000
								},
								new PredictionParameters
								{
									File = Environment.CurrentDirectory + @"\src\Bigramms\bigramm_1-2017-12-2016.json.gz",
									CardCount = 10,
									StepWidth = 2,
									DeckCount = 1,
									SetCount = 3,
									LeafCount = 5,
									SimulationDepth = 5,
								});
							break;
						case CardClass.WARLOCK:
							agent = new PredatorMctsAgent(scoring,
								new MctsParameters
								{
									SimulationTime = simulationTime,
									AggregationTime = 100,
									RolloutDepth = 5,
									UCTConstant = 9000
								},
								new PredictionParameters
								{
									File = Environment.CurrentDirectory + @"\src\Bigramms\bigramm_3-2018-10-2017.json.gz",
									CardCount = 10,
									StepWidth = 2,
									DeckCount = 1,
									SetCount = 3,
									LeafCount = 5,
									SimulationDepth = 1,
								});
							break;
					}
					break;
			};
			return agent;
		}
    }
}
