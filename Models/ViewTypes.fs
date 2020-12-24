﻿module Models.ViewTypes

open Models.CoreTypes
open Models.ValidationTypes

type TextFormat =
    | Highlight
    | Normal

type TextSpan = { Format: TextFormat; Text: string }

type FormattedText = FormattedText of TextSpan list

type TimeSpanEstimate =
    | Days of int
    | Weeks of int
    | Months of int