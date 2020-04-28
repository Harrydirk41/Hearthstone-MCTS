using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SabberStoneAICompetition.PartialObservation;

namespace SabberStoneAICompetition.MCTSAlgorithm.MCTS
{
    abstract class AbstractMctsSimulator
    {
	    /// <summary>
		/// The parameters which drive the MCTS simulation.
		/// </summary>
		protected MctsParameters _mctsParameters;

		/// <summary>
		/// TODO: API
		/// </summary>
		public Stopwatch Watch
		{
			private get; set;
		}

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="mctsParameters">the parameters controlling the simulator</param>
		public AbstractMctsSimulator(MctsParameters mctsParameters)
		{
			_mctsParameters = mctsParameters;
		}

		/// <summary>
		/// TODO: API
		/// </summary>
		/// <param name="game"></param>
		/// <returns></returns>
		public abstract MctsNode Simulate(POGame game);

		/// <summary>
		/// Performs the MCTS simulation for to given root node and returns the best child.
		/// </summary>
		/// <param name="simulationTime"></param>
		/// <param name="root">the root node the MCTS is performed on</param>
		/// <param name="leafNodes">all leaf nodes of the MCTS simulation</param>
		/// <returns>the root's best child node</returns>
		protected MctsNode Simulate(double simulationTime, MctsNode root, ref List<MctsNode> leafNodes)
		{
			// simulate
			while (Watch.Elapsed.TotalMilliseconds <= simulationTime)
			{
				// SELECTION using UCT as Tree-Policy
				MctsNode selectedNode = Select(root);

				MctsNode leafNode = selectedNode;
				if (!leafNode.IsEndTurn)
				{
					// EXPANDATION
					expand(selectedNode);
					// SIMULATION using greedy as Default Policy
					leafNode = Playout( selectedNode);
				}

				// BACKPROPAGATE
				backpropagate(leafNode, leafNode.Score);


				if (selectedNode.IsEndTurn && !leafNodes.Contains(selectedNode))
				{
					leafNodes.Add(selectedNode);
				}
			}

			if (root.Children == null)
			{
				expand(root);
			}

			if (root.Children.Count < 1)
			{
				return root;
			}

			return root.Children
				.OrderByDescending(c => c.TotalScore)
				.First();
		}

		/// <summary>
		/// The selection step of the MCTS. Returns the leaf node with the currently best uct score.
		/// </summary>
		/// <param name="root">the root node the MCTS is performed on</param>
		/// <returns>the leaf node with the best uct score</returns>
		protected MctsNode Select(MctsNode root)
		{
			MctsNode selectedNode = root;
			while (!selectedNode.IsLeaf)
			{
				selectedNode = selectedNode
					.Children.OrderByDescending(c => CalculateUct(c)).First();

			}
			return selectedNode;
		}

		/// <summary>
		/// The expanditation step of the MCTS which adds all children to the selected leaf node.
		/// </summary>
		/// <param name="selectedNode">the selected leaf node</param>
		protected void expand(MctsNode selectedNode)
		{
			// TODO: add something like a expansion-threshold
			selectedNode.Children = selectedNode
				.Tasks.Select(t => new MctsNode(selectedNode.Game, t, selectedNode)).ToList();
		}

		/// <summary>
		/// The rollout step of the MCTS.
		/// </summary>
		/// <param name="expandedNode">the expanded node the simulation is performed on</param>
		/// <returns>the leaf node of the simulation</returns>
		protected MctsNode Playout(MctsNode expandedNode)
		{
			// select child for simulation
			MctsNode child;
			if (expandedNode.Children.Count < 1)
			{
				return expandedNode;
			}

			MctsNode simulationChild = child = expandedNode
				.Children.OrderByDescending(c => c.Score).First();

			// simulate game
			int i = 0;
			while (i++ < _mctsParameters.RolloutDepth
				&& (!simulationChild.IsEndTurn && simulationChild.IsRunning))
			{
				expand(simulationChild);
					// greedy
					simulationChild = simulationChild
									.Children.OrderByDescending(c => c.Score).First();
			}

			if (child.Children?.Any() ?? false)
			{
				child.Children = null;
			}

			return simulationChild;
		}

		/// <summary>
		/// The backpropagation step of MCTS.
		/// </summary>
		/// <param name="leafNode">the leaf node the rollout has calculated</param>
		protected void backpropagate(MctsNode leafNode, double score)
		{
			MctsNode parent = leafNode;
			while (parent != null)
			{
				parent.VisitCount++;
				//parent.TotalScore += score;
				// only highest score should be backpropagated
				if (parent.TotalScore < score)
				{
					parent.TotalScore = score;
				}

				parent = parent.Parent;
			}
		}

		/// <summary>
		/// Calculates the uct score for a given node.
		/// </summary>
		/// <param name="node">the node the uct is calculated for</param>
		/// <returns>the uct score </returns>
		private double CalculateUct(MctsNode node)
		{
			// value form the heuristic
			double heuristic = node.TotalScore; ;
			// the exploration parameter — theoretically equal to √2
			double constant = _mctsParameters.UCTConstant; // Math.sqrt(2);
														   // the actual ucb value
			double uct = constant * Math.Sqrt(2 *
				(Math.Log(node.Parent.VisitCount) / node.VisitCount));
			return heuristic + uct;
		}
	}
}
