namespace TimeMeTaskAgent
{
    partial class ScheduledAgent
    {
        //Check if text position is using
        bool TextPositionUsed(Setting_TextPositions Pos)
        {
            try
            {
                if (setDisplayPosition1 == (int)Pos) { return true; }
                else if (setDisplayPosition2 == (int)Pos) { return true; }
                else if (setDisplayPosition3 == (int)Pos) { return true; }
                else if (setDisplayPosition4 == (int)Pos) { return true; }
                return false;
            }
            catch { return false; }
        }

        //Set a text position to string
        void TextPositionSet(Setting_TextPositions Pos, string Text)
        {
            try
            {
                if (setDisplayPosition1 == (int)Pos) { DisplayPosition1Text = Text; }
                if (setDisplayPosition2 == (int)Pos) { DisplayPosition2Text = Text; }
                if (setDisplayPosition3 == (int)Pos) { DisplayPosition3Text = Text; }
                if (setDisplayPosition4 == (int)Pos) { DisplayPosition4Text = Text; }
            }
            catch { }
        }
    }
}