using System.Collections.Generic;
using SabberStoneAICompetition.MCTSAlgorithm.MCTS;
using SabberStoneAICompetition.PartialObservation;
using SabberStoneAICompetition.Score;
using SabberStoneCore.Tasks.PlayerTasks;

namespace SabberStoneAICompetition.MCTSAlgorithm
{
	internal abstract class AbstractMctsAgent : AbstractAgentExt
	{
		protected MctsParameters _mctsParameters;

		private AbstractMctsSimulator _simulator;
		public AbstractMctsAgent(IScore scoring, MctsParameters mctsParameters)
			: base(scoring)
		{
			_mctsParameters = mctsParameters;
			//_predictionParameters = predictionParameters;

			//if (_predictionParameters != null)
			//{
			//	_map = BigramMapReader.ParseFile(_predictionParameters.File); ;
			//}
		}

		protected override List<PlayerTask> GetSolutions(POGame game, int playerId, IScore scoring)
		{
			// lazily instantiate
			if (_simulator == null)
			{
				//_simulator = new MCTSSimulator(_mctsParameters, _predictionParameters,
				//	playerID, scoring, _map)
				//{
				//	Watch = Watch
				//};
				_simulator = InitSimulator(playerId, scoring);
			}

			if (Watch.Elapsed.TotalMilliseconds <= (_mctsParameters.SimulationTime - _mctsParameters.AggregationTime))
			{
				return _simulator.Simulate(game).GetSolution();
			}

			// safety net, when everything goes wrong
			return new List<PlayerTask> { EndTurnTask.Any(game.CurrentPlayer) };
		}

		protected abstract AbstractMctsSimulator InitSimulator(int playerId, IScore scoring);

	}
}
