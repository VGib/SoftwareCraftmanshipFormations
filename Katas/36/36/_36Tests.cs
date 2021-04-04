using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _36
{
    public class _36
    {
        public const int MaxGameScore = 36;

        private const int NoPlayer = -1;

        private readonly List<string> _players = new();
        private int _playerTurnIndex = 0;
        private int _winnerIndex = NoPlayer;
        private int _firstLooserIndex = NoPlayer;

        public int CumulativeScore { get; private set; } = 0;

        public void AddPlayer(string player)
        {
            if(GameState != _36GameState.IsEnteringPlayers)
            {
                throw new _36Exception("can not add player when game has started");
            }

            _players.Add(player);
        }

        public void StartGame()
        {
            if (GameState != _36GameState.IsEnteringPlayers)
            {
                throw new _36Exception("game has already started!");
            }

            GameState = _36GameState.Playing;
        }

        public void Roll(int diceValue)
        {
            CheckPlayerCanRoll(diceValue);

            if (CumulativeScore + diceValue == MaxGameScore)
            {
                GameState = _36GameState.End;
                _winnerIndex = _playerTurnIndex;
                _playerTurnIndex = NoPlayer;
            }
            else if (CumulativeScore + diceValue > MaxGameScore)
            {
                if (_firstLooserIndex == NoPlayer)
                {
                    _firstLooserIndex = _playerTurnIndex;
                }

                else if (_firstLooserIndex == NextPlayer(_playerTurnIndex))
                {
                    GameState = _36GameState.End;
                    _playerTurnIndex = NoPlayer;
                }
            }
            else
            {
                CumulativeScore += diceValue;
            }

            if (_playerTurnIndex != NoPlayer)
            {
                _playerTurnIndex = NextPlayer(_playerTurnIndex);
            }
        }

        private void CheckPlayerCanRoll(int diceValue)
        {
            if (GameState != _36GameState.Playing)
            {
                throw new _36Exception("can only roll when game has begun");
            }

            if (diceValue < 1 || diceValue > 6)
            {
                throw new _36Exception("dice value should be between 1 and 6");
            }
        }

        private int NextPlayer(int playerIndex)
        {
            ++playerIndex;
            if (playerIndex >= _players.Count)
            {
                playerIndex = 0;
            }

            return playerIndex;
        }

        public _36GameState GameState { get; private set; } = _36GameState.IsEnteringPlayers; 

        public IList<string> Players => _players.ToArray();

        public string PlayerTurn
        {
            get {
                if(GameState != _36GameState.Playing)
                {
                    throw new _36Exception("Only avalaible during game!");
                }

                return _players[_playerTurnIndex];
            }
        }
        public string Winner
        {
            get
            {
                if(GameState != _36GameState.End)
                {
                    throw new _36Exception("the party didn't end!");
                }

                if(_winnerIndex == NoPlayer)
                {
                    throw new _36Exception("there is no winner, please don't forget to check HasWinner property");
                }

                return _players[_winnerIndex];
            }
        }

        public IList<string> LostPlayers
        {
            get
            {
                return EnumerateOnLostPlayers().ToArray();
            }
        }

        public bool HasWinner
        {
            get
            {
               if(GameState != _36GameState.End)
                {
                    throw new _36Exception("the party didn't end!");
                }

               return _winnerIndex != NoPlayer;
            }
        }

        private IEnumerable<string> EnumerateOnLostPlayers()
        {
            if(_firstLooserIndex != NoPlayer)
            {
                var index = _firstLooserIndex;

                do
                {
                    yield return _players[index];
                    index = NextPlayer(index);
                }
                while (index != _playerTurnIndex && index != _firstLooserIndex
                && (_winnerIndex == NoPlayer || _winnerIndex != index));
            }
        }
    }

    public enum _36GameState
    {
        IsEnteringPlayers,
        Playing,
        End
    }

    [Serializable]
    public class _36Exception : Exception
    {
        public _36Exception() { }
        public _36Exception(string message) : base(message) { }
        public _36Exception(string message, Exception inner) : base(message, inner) { }
        protected _36Exception(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    public class _36Tests
    {
        [Fact]
        public void At_the_begining_we_add_the_players()
        {
            var target = CreateTarget();

            Assert.Equal(3, target.Players.Count);
            Assert.Equal("toto", target.Players[0]);
            Assert.Equal("titi", target.Players[1]);
            Assert.Equal("tutu", target.Players[2]);
        }

        private static _36 CreateTarget()
        {
            var target = new _36();
            target.AddPlayer("toto");
            target.AddPlayer("titi");
            target.AddPlayer("tutu");
            return target;
        }

        [Fact]
        public void Can_not_add_player_when_the_game_has_begin()
        {
            var target = CreateTarget();
            target.StartGame();
            Assert.Throws<_36Exception>(() => target.AddPlayer("zuzu"));
        }

        [Fact]
        public void when_adding_players_the_game_state_should_be_entering_players()
        {
            var target = CreateTarget();
            Assert.Equal(_36GameState.IsEnteringPlayers, target.GameState);
        }

        [Fact]
        public void when_game_has_started_the_state_should_be_Playing()
        {
            var target = CreateTarget();
            target.StartGame();
            Assert.Equal(_36GameState.Playing, target.GameState);
        }

        [Fact]
        public void starting_game_when_game_has_already_started_is_not_possible()
        {
            var target = CreateTarget();
            target.StartGame();
            Assert.Throws<_36Exception>(() => target.StartGame());
        }

        [Fact]
        public void starting_game_when_game_has_already_end_is_not_possible()
        {
            var target = CreateTarget();
            target.StartGame();
            Assert.Throws<_36Exception>(() => target.Roll(0));
        }

        [Fact]
        public void starting_game_when_game_has_already_end_is_not_possible_2()
        {
            var target = CreateTarget();
            target.StartGame();
            Assert.Throws<_36Exception>(() => target.Roll(7));
        }

        [Fact]
        public void Players_should_play_by_adding_order()
        {
            var target = CreateTarget();
            target.StartGame();
            Assert.Equal("toto", target.PlayerTurn); 
        }

        [Fact]
        public void Players_should_play_by_adding_order_2()
        {
            var target = CreateTarget();
            target.StartGame();
            target.Roll(1);
            Assert.Equal("titi", target.PlayerTurn);
        }

        [Fact]
        public void Players_should_play_by_adding_order_3()
        {
            var target = CreateTarget();
            target.StartGame();
            target.Roll(1);
            target.Roll(2);
            Assert.Equal("tutu", target.PlayerTurn);
        }

        [Fact]
        public void Players_should_play_by_adding_order_4()
        {
            var target = CreateTarget();
            target.StartGame();
            target.Roll(1);
            target.Roll(2);
            target.Roll(2);
            Assert.Equal("toto", target.PlayerTurn);
        }

        [Fact]
        public void If_a_dice_value_should_be_between_1_and_6()
        {
            var target = CreateTarget();
            target.StartGame();
            Assert.Throws<_36Exception>(() =>  target.Roll(0));
        }

        [Fact]
        public void If_a_dice_value_should_be_between_1_and_2()
        {
            var target = CreateTarget();
            target.StartGame();
            Assert.Throws<_36Exception>(() => target.Roll(0));
        }

        [Fact]
        public void If_a_roll_is_less_than_36_then_the_cumulate_score_is_updated()
        {
            var target = CreateTarget();
            target.StartGame();
            target.Roll(1);
            target.Roll(5);
            target.Roll(4);
            target.Roll(6);

            Assert.Equal(16, target.CumulativeScore);
        }

        [Fact]
        public void At_the_begining_of_the_party_cumulative_score_should_be_zero()
        {
            var target = CreateTarget();
            Assert.Equal(0, target.CumulativeScore);
        }

        [Fact]
        public void a_player_can_not_roll_when_the_party_has_not_begun()
        {
            var target = CreateTarget();
            Assert.Throws<_36Exception>(() => target.Roll(2));
        }

        [Fact]
        public void a_player_can_not_roll_when_the_party_has_end()
        {
            var target = CreateTarget();
            target.StartGame();
            RepeatRoll(target, 6, 6);
            Assert.Equal(_36GameState.End, target.GameState);
            Assert.Throws<_36Exception>(() => target.Roll(2));
        }

        [Fact]
        public void If_a_player_roll_and_cumulate_score_is_36_the_player_won()
        {
            var target = CreateTarget();
            target.StartGame();
            RepeatRoll(target,5, 6);

            Assert.Equal("tutu", target.PlayerTurn);

            target.Roll(6);

            Assert.Equal(_36GameState.End, target.GameState);
            Assert.Equal("tutu", target.Winner);
        }

        private static void RepeatRoll(_36 target, int numberOfRepeat, int eachDiceValue)
        {
            for (int repetition = 0; repetition < numberOfRepeat; repetition++)
            {
                target.Roll(eachDiceValue);
            }
        }

        [Fact]
        public void winner_should_not_be_available_if_game_is_not_finished()
        {
            var target = CreateTarget();
            target.StartGame();
            RepeatRoll(target, 5, 6);
            Assert.Equal(_36GameState.Playing, target.GameState);
            Assert.Throws<_36Exception>(() => target.Winner);
        }

        [Fact]
        public void winner_should_not_be_available_if_game_is_not_finished_2()
        {
            var target = CreateTarget();
            Assert.Equal(_36GameState.IsEnteringPlayers, target.GameState);
            Assert.Throws<_36Exception>(() => target.Winner);
        }

        [Fact]
        public void If_a_launch_is_more_than_36_then_the_cumulate_player_lost()
        {
            var target = CreateTarget();
            target.StartGame();
            RepeatRoll(target, 8, 4);
            Assert.Equal("tutu", target.PlayerTurn);

            target.Roll(5);
            AssertPlayerHasLost(target, "tutu");
        }

        [Fact]
        public void If_a_launch_is_more_than_36_then_the_cumulate_player_lost_2()
        {
            var target = CreateTarget();
            target.StartGame();
            RepeatRoll(target, 8, 4);
            target.Roll(5);
            Assert.Equal("toto", target.PlayerTurn);

            target.Roll(5);
            Assert.Equal(2, target.LostPlayers.Count);
            AssertPlayerHasLost(target, "toto");
        }

        private static void AssertPlayerHasLost(_36 target, string player)
        {
            Assert.Contains(target.LostPlayers, x => x == player);
        }

        [Fact]
        public void If_a_player_has_won_he_should_not_be_in_LostPlayers()
        {
            var target = CreateTarget();
            target.StartGame();
            RepeatRoll(target, 8, 4);
            target.Roll(5);
            Assert.Equal("toto", target.PlayerTurn);

            target.Roll(4);
            Assert.Equal(1, target.LostPlayers.Count);
            AssertPlayerHasNotLost(target, "toto");
        }

        private static void AssertPlayerHasNotLost(_36 target, string player)
        {
            Assert.DoesNotContain(target.LostPlayers, x => x == player);
        }

        [Fact]
        public void If_no_player_has_lost_the_game_LostPlayers_should_be_empty()
        {
            var target = CreateTarget();
            target.StartGame();
            RepeatRoll(target, 12, 2);
            Assert.Equal(0, target.LostPlayers.Count);
        }



        [Fact]
        public void If_all_player_has_lost_the_game_LostPlayers_should_contains_all_players()
        {
            var target = CreateTarget();
            target.StartGame();
            RepeatRoll(target, 8, 4);
            RepeatRoll(target, 3, 5);

            Assert.Equal(3, target.LostPlayers.Count);
            AssertPlayerHasLost(target, "toto");
            AssertPlayerHasLost(target, "titi");
            AssertPlayerHasLost(target, "tutu");
        }

        [Fact]
        public void If_a_player_has_lost_the_game_he_cant_play_anymore()
        {
            var target = CreateTarget();
            target.StartGame();
            RepeatRoll(target, 8, 4);
            RepeatRoll(target, 3, 5);

            Assert.Throws<_36Exception>(() => target.Roll(3));
        }

        [Fact]
        public void If_a_player_has_won_the_game_has_ended()
        {
            var target = CreateTarget();
            target.StartGame();
            RepeatRoll(target, 6, 6);
            Assert.Equal(_36GameState.End, target.GameState);
        }

        [Fact]
        public void If_a_player_has_won_he_is_the_winner()
        {
            var target = CreateTarget();
            target.StartGame();
            RepeatRoll(target, 5, 6);

            Assert.Equal("tutu", target.PlayerTurn);

            target.Roll(6);

            Assert.Equal(_36GameState.End, target.GameState);
            Assert.Equal("tutu", target.Winner);
        }

        [Fact]
        public void If_a_player_has_won_he_is_the_winner_2()
        {
            var target = CreateTarget();
            target.StartGame();
            RepeatRoll(target, 5, 6);

            Assert.Equal("tutu", target.PlayerTurn);

            target.Roll(6);

            Assert.Equal(_36GameState.End, target.GameState);
            Assert.True(target.HasWinner);
        }

        [Fact]
        public void If_all_player_has_lost_the_game_has_ended()
        {
            var target = CreateTarget();
            target.StartGame();
            RepeatRoll(target, 8, 4);
            RepeatRoll(target, 3, 5);

            Assert.Equal(_36GameState.End, target.GameState);
        }

        [Fact]
        public void If_all_player_has_lost_there_is_no_winner()
        {
            var target = CreateTarget();
            target.StartGame();
            RepeatRoll(target, 8, 4);
            RepeatRoll(target, 3, 5);

            Assert.Equal(_36GameState.End, target.GameState);
            Assert.False(target.HasWinner);
        }

        [Fact]
        public void If_all_player_has_lost_there_is_no_winner_2()
        {
            var target = CreateTarget();
            target.StartGame();
            RepeatRoll(target, 8, 4);
            RepeatRoll(target, 3, 5);

            Assert.Equal(_36GameState.End, target.GameState);
            Assert.Throws<_36Exception>(() => target.Winner);
        }

        [Fact]
        public void Can_not_check_HasWinner_if_game_has_not_end()
        {
            var target = CreateTarget();
            Assert.Throws<_36Exception>(() => target.HasWinner);
        }

        [Fact]
        public void Can_not_check_HasWinner_if_game_has_not_end_2()
        {
            var target = CreateTarget();
            target.StartGame();
            target.Roll(1);
            Assert.Throws<_36Exception>(() => target.HasWinner);
        }

        [Fact]
        public void If_game_has_not_started_its_no_players_turn()
        {
            var target = CreateTarget();
            Assert.Throws<_36Exception>(() => target.PlayerTurn);
        }

        [Fact]
        public void If_game_has_end_its_no_players_turn()
        {
            var target = CreateTarget();
            target.StartGame();
            RepeatRoll(target, 6, 6);
            Assert.Equal(_36GameState.End, target.GameState);
            Assert.Throws<_36Exception>(() => target.PlayerTurn);
        }
    }
}
