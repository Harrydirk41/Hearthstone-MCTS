﻿
namespace SabberStoneAICompetition.MCTSAlgorithm.MCTS
{
	/// <summary>
	/// Container class to encapsulate all prediction specific parameters.
	/// </summary>
    public class PredictionParameters
    {
		/// <summary>
		///
		/// </summary>
		public string File
		{ get; set; }


		/// <summary>
		/// Returns the number of cards which are picked from a prediction.
		/// </summary>
		public int CardCount
		{ get; set; }

		/// <summary>
		/// Returns the number of decks used for prediction.
		/// </summary>
		public int DeckCount
		{ get; set; }

		/// <summary>
		/// TODO: API
		/// </summary>
		public int StepWidth
		{ get; set; }

		/// <summary>
		/// Returns the number of sets consisting of hand and deck cards which are
		/// created using different permutations of predicted cards.
		/// </summary>
		public int SetCount
		{ get; set; }

		/// <summary>
		/// Returns the number of leafs, aka end turn nodes, on which the prediction driven MCTS will run.
		/// For each node, two MCTS simulation will be performed sequentially: one for the opponent; one for the player.
		/// </summary>
		public int LeafCount
		{ get; set; }

		/// <summary>
		///
		/// </summary>
		public int SimulationDepth
		{ get; set; }

		public override string ToString()
		{
			return $"CardCount: {CardCount} StepWidth: {StepWidth} SetCount: {SetCount} LeafPercentage: {LeafCount}";
		}
	}
}
