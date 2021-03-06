﻿//Copyright 2019 Volodymyr Podshyvalov
//
//Licensed under the Apache License, Version 2.0 (the "License");
//you may not use this file except in compliance with the License.
//You may obtain a copy of the License at
//
//    http://www.apache.org/licenses/LICENSE-2.0
//
//Unless required by applicable law or agreed to in writing, software
//distributed under the License is distributed on an "AS IS" BASIS,
//WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//See the License for the specific language governing permissions and
//limitations under the License.

namespace WpfHandler.UI.Controls
{
    /// <summary>
    /// Implement that interface for controls with existed text label.
    /// </summary>
    public interface ILabel
    {
        /// <summary>
        /// Text of the label.
        /// </summary>
        string Label { get; set; }

        /// <summary>
        /// Width of label field.
        /// </summary>
        float LabelWidth { get; set; }
    }
}
