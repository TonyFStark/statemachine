//-------------------------------------------------------------------------------
// <copyright file="StateDefinitionsBuilder.cs" company="Appccelerate">
//   Copyright (c) 2008-2019 Appccelerate
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
// </copyright>
//-------------------------------------------------------------------------------

namespace Appccelerate.StateMachine.Facts.Machine
{
    using System;
    using System.Collections.Generic;
    using StateMachine.Machine;
    using StateMachine.Machine.States;
    using StateMachine.Syntax;

    public class StateDefinitionsBuilder<TState, TEvent>
        where TState : IComparable
        where TEvent : IComparable
    {
        private readonly List<Func<ISyntaxStart<TState, TEvent>, object>> setupFunctions = new List<Func<ISyntaxStart<TState, TEvent>, object>>();

        public StateDefinitionsBuilder<TState, TEvent> WithConfiguration(
            Func<ISyntaxStart<TState, TEvent>, object> setupFunction)
        {
            this.setupFunctions.Add(setupFunction);
            return this;
        }

        public IReadOnlyDictionary<TState, IStateDefinition<TState, TEvent>> Build()
        {
            var stateDefinitionDictionary = new StateDictionary<TState, TEvent>();
            var initiallyLastActiveStates = new Dictionary<TState, IStateDefinition<TState, TEvent>>();
            var syntaxStart = new SyntaxStart<TState, TEvent>(stateDefinitionDictionary, initiallyLastActiveStates);

            this.setupFunctions.ForEach(f => f(syntaxStart));

            return stateDefinitionDictionary.ReadOnlyDictionary;
        }
    }
}