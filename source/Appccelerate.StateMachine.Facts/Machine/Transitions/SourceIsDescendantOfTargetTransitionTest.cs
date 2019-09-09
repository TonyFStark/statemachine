﻿//-------------------------------------------------------------------------------
// <copyright file="SourceIsDescendantOfTargetTransitionTest.cs" company="Appccelerate">
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

namespace Appccelerate.StateMachine.Facts.Machine.Transitions
{
    using FakeItEasy;
    using StateMachine.Machine.States;
    using Xunit;

    public class SourceIsDescendantOfTargetTransitionTest : SuccessfulTransitionWithExecutedActionsTestBase
    {
        private readonly IStateDefinition<States, Events> intermediate;

        public SourceIsDescendantOfTargetTransitionTest()
        {
            this.Target = Builder<States, Events>.CreateStateDefinition().Build();
            this.intermediate = Builder<States, Events>.CreateStateDefinition().WithSuperState(this.Target).Build();
            this.Source = Builder<States, Events>.CreateStateDefinition().WithSuperState(this.intermediate).Build();
            this.TransitionContext = Builder<States, Events>.CreateTransitionContext().WithStateDefinition(this.Source).Build();

            this.TransitionDefinition.Source = this.Source;
            this.TransitionDefinition.Target = this.Target;
        }

        [Fact]
        public void ExitsOfAllStatesFromSourceUpToTarget()
        {
            this.Testee.Fire(this.TransitionDefinition, this.TransitionContext, this.LastActiveStateModifier);

            A.CallTo(() => this.StateLogic.Exit(this.Source, this.TransitionContext, this.LastActiveStateModifier)).MustHaveHappened()
                .Then(A.CallTo(() => this.StateLogic.Exit(this.intermediate, this.TransitionContext, this.LastActiveStateModifier)).MustHaveHappened())
                .Then(A.CallTo(() => this.StateLogic.Exit(this.Target, this.TransitionContext, this.LastActiveStateModifier)).MustHaveHappened());
        }

        [Fact]
        public void EntersTargetState()
        {
            this.Testee.Fire(this.TransitionDefinition, this.TransitionContext, this.LastActiveStateModifier);

            A.CallTo(() => this.StateLogic.Entry(this.Target, this.TransitionContext)).MustHaveHappened();
        }
    }
}