using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
namespace GemSwap
{
    public class SwipPicGame
    {
        private const int MiliSecondDelayForMoveCell = 5;
        private const int MiliSecondDelayAfterMoveCell = 100;
        private const int MiliSecondDelayForRemoveCell = 5;
        private const int MiliSecondDelayBecauseMoveIncorrecCell = 150;
        private bool _IsGameOver = false;
        private bool IsGameOver
        {
            get
            {
                return _IsGameOver;
            }
        }
        public event Score.ScoreChangeEventHandler ScoreChangeEvent;
        public delegate void GameOverEventHandler();
        public event GameOverEventHandler GameOverEvent;
        private DoubleBufferedPanel CreateDoubleBufferedPanel()
        {
            DoubleBufferedPanel dbf = new DoubleBufferedPanel();
            dbf.BackColor = System.Drawing.Color.White;
            dbf.Location = new System.Drawing.Point(1, 3);
            dbf.Name = "dbf";
            dbf.Size = new System.Drawing.Size(440, 440);
            dbf.TabIndex = 0;

          
            return dbf;
        }
        private enum enDirection
        {
            Up,
            Down,
            Left,
            Right
        }
        private DoubleBufferedPanel _Pnl;
        public DoubleBufferedPanel Pnl
        {
            get { return _Pnl; }
        }
        private int _Row = 0;
        private int _Col = 0;
        private Score _Score = null;
        private Score Score
        {
            get { return _Score; }
        }
        private PanelGrid _PnlGrid;
        private PanelGrid PnlGrid
        {
            get { return _PnlGrid; }
        }
        private Cell FirstCell = null;
        private Cell SecondCell = null;
        private int FirstCellX = 0;
        private int SecondCellX = 0;
        public void DebugBySetFirstCell()
        {
            this.PnlGrid.Table.Rows[0].Cols[0].Value = "1";

        }
        private bool IsTheseTheSameCell(Cell c, Cell c2)
        {
            if (c.Position.X != c2.Position.X ||
                c.Position.Y != c2.Position.Y)
            {
                return false;
            }

            return true;
        }
        private enDirection GetMoveDirection()
        {
            if (FirstCell.Position.X < SecondCell.Position.X)
            {
                return enDirection.Right ;
            }

            if (FirstCell.Position.X > SecondCell.Position.X)
            {
                return enDirection.Left  ;
            }

            if (FirstCell.Position.Y < SecondCell.Position.Y)
            {
                return enDirection.Down;
            }

            if (FirstCell.Position.Y < SecondCell.Position.Y)
            {
                return enDirection.Up;
            }
            return enDirection.Up;
        }
        private bool IsFirstCell(Cell c)
        {
            if (FirstCell == null)
            {
                return false;
            }

            return IsTheseTheSameCell(FirstCell, c);
        }
        private bool IsSecondCell(Cell c)
        {
            if (SecondCell == null)
            {
                return false;
            }

            return IsTheseTheSameCell(SecondCell, c);
        }
        private Bitmap GetBitFromCell(Cell c)
        {
            string FileName = Util.CurrentPath + @"Images\gem" + c.Value + ".png";
            Bitmap BOriginalSize = new Bitmap(FileName);
            Bitmap BResized = new Bitmap(BOriginalSize, new System.Drawing.Size(this.PnlGrid.CellSize, this.PnlGrid.CellSize));
            return BResized;
        }

        private Bitmap GetBitFromCellForSmaller(Cell c)
        {

            string FileName = Util.CurrentPath + @"Images\gem" + c.Value + ".png";
            Bitmap BOriginalSize = new Bitmap(FileName);
            Bitmap BResized = new Bitmap(BOriginalSize, new System.Drawing.Size(this.PnlGrid.CellSize /2, this.PnlGrid.CellSize /2));
            return BResized;
        }

        private void RenderFallingCell()
        {
            eRenderType = enRenderType.Falling;
            this.PnlGrid.Invalidate();
            this.PnlGrid.Update();
        }
        public  void RenderNewBackGround()
        {
            eRenderType = enRenderType.NewBackGround;
            
            this.PnlGrid.Update();
            this.PnlGrid.Invalidate();
        }
        private void RenderRainingCell()
        {
            eRenderType = enRenderType.Raining;
            this.PnlGrid.Invalidate();
            this.PnlGrid.Update();
        }
        private void RenderTable()
        {
            eRenderType = enRenderType.WholeTable;
            this.PnlGrid.Invalidate();
            this.PnlGrid.Update();
        }
        private void RenderOnlyCell(Cell c)
        {
            this.PnlGrid.Invalidate(c);
            this.PnlGrid.Update();
        }
        private void RenderFirstAndSecondCell()
        {
            Util.WriteLog("   RenderFirstAndSecondCell");
            eRenderType = enRenderType.Swaping;
            this.PnlGrid.Invalidate(FirstCell);
            this.PnlGrid.Invalidate(SecondCell);
            this.PnlGrid.Update();
            

        }
        private void DelayForMoveCell()
        {
            Delay(5);
        }
        private void DelayAfterMovedCell()
        {
            Delay(15);
        }
        private void DelayAfterReMovedCell()
        {
            Delay(10);
        }

        private void DelayForFallingCell()
        {
            Delay(1);
        }
        private void DelayForRemoveCell()
        {
            Delay(10);
        }
        private void DelayBecauseMovedIncorrectCell()
        {
            Delay(75);
        }
        private void Delay(int milisecond)
        {
            System.Threading.Thread.Sleep(milisecond);
        }
        private Rectangle GetRecFromCell(Cell c)
        {
            Rectangle r = new Rectangle(c.Position.X * this.PnlGrid.CellSize, c.Position.Y * this.PnlGrid.CellSize, this.PnlGrid.CellSize, this.PnlGrid.CellSize);
            return r;
        }
        private enum enRenderType
        {
            WholeTable,
            Swaping,
            RemoveCell,
            Falling,
            Raining,
            NewBackGround
        }
        private enRenderType eRenderType = enRenderType.WholeTable;
        public void cP_PanelPaintEvent(PaintEventArgs PE)
        {
            
            
            Graphics g = PE.Graphics;
            
                
            Bitmap B = new Bitmap(Util.CurrentPath + @"Images\gem1.png");
        //    cUtil.WriteLog("cp_PanelPaintEvent " + eRenderType.ToString ());
            int i;
            int j;
            Bitmap BRender = null;
            Point P;
            Util.WriteLog("eRenderType:" + eRenderType.ToString());

            switch (eRenderType)
            {
                case enRenderType.NewBackGround :
                    g.Clear(this.PnlGrid.P.BackColor);
                    break;
                case  enRenderType.Swaping :
                         P = new Point(PFirst.X, PFirst.Y);
                        BRender = GetBitFromCell(FirstCell);
                        g.DrawImage(BRender, P);

                        P = new Point(PSecond.X, PSecond.Y);
                        BRender = GetBitFromCell(SecondCell);
                        g.DrawImage(BRender, P);
                    break;
                case  enRenderType.WholeTable :
                    foreach (Cell c in this.PnlGrid.Table.Cells)
                    {

                        if (c.Value != "")
                        {
                            BRender = GetBitFromCell(c);

                            P = new Point(c.Position.X * BRender.Size.Width, c.Position.Y * BRender.Size.Height);                         
                            g.DrawImage(BRender, P);
                        }

                        Pen MyPen = null;
                        if (IsFirstCell(c))
                        {
                            MyPen = new Pen(Color.Teal, 4);
                        }
                        else
                        {
                            MyPen = new Pen(Color.Transparent, 4);
                        }
                        g.DrawRectangle(MyPen, GetRecFromCell(c));


                    }
                    break;
                case enRenderType.RemoveCell :
                    
                    for (i = 0; i < lstRemoveCell.Count; i++)
                    {
                        Cell c = this.PnlGrid.Table.Rows[lstRemoveCell[i].Y].Cols[lstRemoveCell[i].X];
                        if (c.Value != "")
                        {
                            BRender = GetBitFromCellForSmaller(c);                            
                            P = new Point(c.Position.X * BRender.Size.Width, c.Position.Y * BRender.Size.Height);
                            g.DrawImage(BRender, P);
                        }
                    }
                    break;
                case  enRenderType.Falling :
                    for (i = 0; i < lstMovingCell.Count; i++)
                    {
                        if (lstMovingCell[i].Cell.Value != "")
                        {
                            P = new Point(lstMovingCell[i].RenderPosition.X, lstMovingCell[i].RenderPosition.Y);
                        //    BRender = GetBitFromCell(lstMovingCell[i].Cell);
                            g.DrawImage(lstMovingCell[i].B , P);
                        }
                    }                  

                    break;
                case enRenderType.Raining :
                    
                    for (i = 0; i < lstStandStillCell.Count; i++)
                    {
                        Cell c = lstStandStillCell[i].Cell;
                        P = new Point(c.Position.X * this.PnlGrid.CellSize, c.Position.Y * this.PnlGrid.CellSize);
                        g.DrawImage(lstStandStillCell[i].B, P);
                    }
                    for (i = 0; i < lstRainingCell.Count; i++)
                    {
                        if (!lstRainingCell[i].HasReachedGoal)
                        {
                            P = new Point(lstRainingCell[i].RenderPosition.X, lstRainingCell [i].RenderPosition.Y);
                            g.DrawImage(lstRainingCell[i].B, P);
                        }
                    }
                    break;
            }
         
          

        }
        List<Point> lstRemoveCell = null;

        private bool IsValidSecondCellClick(Cell pSecondCell)
        {
            if (FirstCell == null)
            {
                return false;
            }

            if (pSecondCell.Position.X == FirstCell.Position.X - 1 ||
                pSecondCell.Position.X == FirstCell.Position.X + 1)
            {
                if (pSecondCell.Position.Y == FirstCell.Position.Y)
                {
                    return true;
                }
            }

            if (pSecondCell.Position.Y == FirstCell.Position.Y - 1 ||
                pSecondCell.Position.Y == FirstCell.Position.Y + 1)
            {
                if (pSecondCell.Position.X == FirstCell.Position.X)
                {
                    return true;
                }
            }

            return false;
        }


        private bool IsThisCellNeedToMove(Point P,Dictionary<Point,string> pDic)
        {
            int i;
            int j;
            int iTempSameValueInSameRow = 0;
            int iTempSameValueInSameCol = 0;
            int FirstColumn = 0;
            int FirstRow = 0;

            int LastRow = this.PnlGrid.Table.Rows.Count - 1;
            int LastColumn = this.PnlGrid.Table.Rows[0].Cols.Count - 1;
            int CurrentCol = P.X;
            int CurrentRow = P.Y;
            Point Point2 = new Point();
            for (i = CurrentCol - 1; i >= FirstColumn; i--)
            {

                Point2 = new Point(i, CurrentRow);

                if (pDic [P]== pDic [Point2])
                {
                    iTempSameValueInSameRow++;
                }
                else
                {
                    break;
                }
            }

            for (i = CurrentCol + 1; i <= LastColumn; i++)
            {
                Point2 = new Point(i, CurrentRow);

                if (pDic[P] == pDic[Point2])
                {
                    iTempSameValueInSameRow++;
                }
                else
                {
                    break;
                }
            }
            if (iTempSameValueInSameRow >= 2)
            {
                return true;
            }
            for (i = CurrentRow - 1; i >= FirstRow; i--)
            {
                Point2 = new Point(CurrentCol , i);

                if (pDic[P] == pDic[Point2])
                {
                    iTempSameValueInSameCol++;
                }
                else
                {
                    break;
                }
            }

            for (i = CurrentRow + 1; i <= LastRow; i++)
            {
                Point2 = new Point(CurrentCol, i);

                if (pDic[P] == pDic[Point2])
                {
                    iTempSameValueInSameCol++;
                }
                else
                {
                    break;
                }
            }

            if (iTempSameValueInSameCol >= 2)
            {
                return true;
            }

            return false;
        }

        private bool IsThisCellNeedToMove(Cell c)
        {
            int i;
            int j;
            int iTempSameValueInSameRow = 0;
            int iTempSameValueInSameCol = 0;
            int FirstColumn=0;
            int FirstRow =0;
            
            int LastRow = this.PnlGrid.Table.Rows.Count -1;
            int LastColumn = this.PnlGrid.Table.Rows [0].Cols.Count -1;
            int CurrentCol = c.Position.X ;
            int CurrentRow = c.Position.Y ;
            for (i = CurrentCol  - 1; i >= FirstColumn ; i--)
            {
                if (PnlGrid.Table.Rows[CurrentRow].Cols[i].Value == c.Value)
                {
                    iTempSameValueInSameRow++;
                }
                else
                {
                    break;
                }
            }

            for (i = CurrentCol  + 1; i <= LastColumn  ; i++)
            {
                if (PnlGrid.Table.Rows[CurrentRow].Cols[i].Value == c.Value)
                {
                    iTempSameValueInSameRow++;
                }
                else
                {
                    break;
                }
            }
            if(iTempSameValueInSameRow >= 2)
            {
                return true;
            }         
            for (i = CurrentRow  - 1; i >= FirstRow  ; i--)
            {
                if (PnlGrid.Table.Rows[i].Cols[CurrentCol ].Value == c.Value)
                {
                    iTempSameValueInSameCol++;
                }
                else
                {
                    break;
                }
            }

            for (i = CurrentRow + 1; i <= LastRow ; i++)
            {
                if (PnlGrid.Table.Rows[i].Cols[CurrentCol].Value == c.Value)
                {
                    iTempSameValueInSameCol++;
                }
                else
                {
                    break;
                }
            }

            if (iTempSameValueInSameCol  >= 2)
            {
                return true;
            }

            return false;
        }


        private List<Point> GetNextToCell(Point P,Dictionary<Point,string> pDic)
        {
            List<Point> lst = new List<Point>();
            int LastCol = this.PnlGrid.Table.Rows[0].Cols.Count - 1;
            int LastRow = this.PnlGrid.Table.Rows.Count - 1;
            Point P2;
            if (P.X > 0)
            {
                P2=new Point (P.X -1,P.Y );
                lst.Add (P2);
                
            }
            if (P.X < LastCol )
            {
                P2 = new Point(P.X+ 1, P.Y);
                lst.Add(P2);

            }

            if (P.Y > 0)
            {
                P2 = new Point(P.X, P.Y-1);
                lst.Add(P2);

            }
            if (P.Y < LastRow )
            {
                P2 = new Point(P.X, P.Y+1);
                lst.Add(P2);

            }
   
            return lst;
        }
        private List<Cell> GetNextToCell(Cell c)
        {
            List<Cell> lst = new List<Cell>();
            int LastCol = this.PnlGrid.Table.Rows[0].Cols.Count - 1;
            int LastRow = this.PnlGrid.Table.Rows.Count - 1;
            if (c.Position.X > 0)
            {
                lst.Add(this.PnlGrid.Table.Rows[c.Position.Y].Cols[c.Position.X - 1]);               
            }
            if (c.Position.X < LastCol )
            {
                lst.Add(this.PnlGrid.Table.Rows[c.Position.Y].Cols[c.Position.X +1]);
            }

            if (c.Position.Y > 0)
            {
                lst.Add(this.PnlGrid.Table.Rows[c.Position.Y-1].Cols[c.Position.X]);
            }
            if (c.Position.Y < LastRow) 
            {
                lst.Add(this.PnlGrid.Table.Rows[c.Position.Y + 1].Cols[c.Position.X]);
            }
            return lst;
        }
        private bool IsValidTable()
        {
            return false;
        }


        private bool IsEndGame(Dictionary<Point,string> pDic)
        {
            int i;
            int j;
            int k;
            foreach (Point P in pDic.Keys)
            {
               // cCell MyFirst = this.PnlGrid.Table.Rows[i].Cols[j];
                //List<cCell> lstNextTo = GetNextToCell(MyFirst);
                List<Point> lstNextTo = GetNextToCell(P,pDic );
                TableCell TC = ConvertFromDicToTableCell(pDic);
                for (k = 0; k < lstNextTo.Count; k++)
                {
                    TableCell TCTemp = TC.Clone();
                    string temp = TCTemp.Cell(P).Value;
                    TCTemp.Cell(P).Value = TCTemp.Cell(lstNextTo[k]).Value;
                    TCTemp.Cell(lstNextTo[k]).Value = temp;



                    AttemptToMoveBlockResult AttemptResultFromFirstCell = null; ;
                    AttemptToMoveBlockResult AttemptResultFromSecondCell = null;
                    AttemptResultFromFirstCell = AttemptToMoveBloack(TC, P);
                    AttemptResultFromSecondCell = AttemptToMoveBloack(TC, lstNextTo [k]);

                    if (AttemptResultFromFirstCell.CanMove ||
                        AttemptResultFromSecondCell.CanMove)
                    {
                        return false;
                    }

                }
            }
           
            return true;


        }
        public bool IsEndGame()
        {
            int i;
            int j;
            int k;
            for (i = 0; i < this.PnlGrid.Table.Rows.Count; i++)
            {
                for (j = 0; j < this.PnlGrid.Table.Rows.Count; j++)
                {

                    Cell MyFirst = this.PnlGrid.Table.Rows[i].Cols[j];
                    List<Cell> lstNextTo = GetNextToCell(MyFirst);
                    for (k = 0; k < lstNextTo.Count; k++)
                    {
                        TableCell TC = GetCloneTableWithAttemptValue(MyFirst , lstNextTo [k]);
                        AttemptToMoveBlockResult AttemptResultFromFirstCell = null; ;
                        AttemptToMoveBlockResult AttemptResultFromSecondCell = null;
                        AttemptResultFromFirstCell = AttemptToMoveBloack(TC, MyFirst.Position );
                        AttemptResultFromSecondCell = AttemptToMoveBloack(TC, lstNextTo [k].Position);

                        if (AttemptResultFromFirstCell.CanMove ||
                            AttemptResultFromSecondCell.CanMove)
                        {
                            return false;
                        }
                      
                    }
                }
            }
            return true;
         

        }
        private List<Point> GetCellNeedToRemoveFromDic(Dictionary<Point,string> pDic)
        {
            int i;
            int j;
            int k;
            int l;
            //HashSet<Point> HashPoint = new HashSet<Point>();
            List<Point> lstP = new List<Point>();
            foreach (Point P in pDic.Keys)
            {
                if(IsThisCellNeedToMove (P,pDic ))
                {
                     lstP.Add(P);
                }
            }
           
            return lstP;
        }
        private List<Point> GetCellNeedToRemove()
        {
            int i;
            int j;
            int k;
            int l;
            //HashSet<Point> HashPoint = new HashSet<Point>();
            List<Point> lstP = new List<Point>();
            for (i = 0; i < this.PnlGrid.Table.Rows.Count; i++)
            {
                for (j = 0; j < this.PnlGrid.Table.Rows[0].Cols.Count ; j++)
                {
                    Cell c = this.PnlGrid.Table.Rows[i].Cols[j];
                    Point P = c.Position;
                   
                    if (IsThisCellNeedToMove(c))
                    {
                        lstP.Add(P);
                    }                   
                }
            }
            return lstP;
        }
        private void cP_CellClickEvent(Cell c)
        {
            //c.Value = "";
            if (IsBeingSwap)
            {
                return;
            }
            if (IsGameOver)
            {
                return;
            }
            if (FirstCell == null)
            {
                FirstCell = c;
            }
            else
            {
                if (IsValidSecondCellClick(c))
                {
                    SecondCell = c;
                }
                else
                {
                    FirstCell = c;
                }
            }

            PnlGrid.Invalidate(c);
            PnlGrid.Update();
            if (SecondCell != null)
            {
               SwapBlockResult SwapResult= SwapBlock(FirstCell, SecondCell);              
               if (SwapResult.CanSwqp)
               {

                   DelayForMoveCell();
                   //RemoveBlockedIntheSameLine();
                   this.lstRemoveCell = new List<Point>();
                   lstRemoveCell.AddRange(SwapResult.lstNeedToRemoveForCell1);
                   lstRemoveCell.AddRange(SwapResult.lstNeedToRemoveForCell2);
                   Score.AddScoreFromRemovedCell(lstRemoveCell.Count, false, 1);
                   RemoveBlocked(lstRemoveCell );
                   
                   FirstCell = null; 
                   SecondCell = null;
                   bool IsThereAnyCellNeedtoBeRemove = true;
                   int iChainLevel = 1;
                   while (IsThereAnyCellNeedtoBeRemove)
                   {
                       FallBlockV2();
                       RainBlock();
                       List<Point> lstRemoveCellAfterRain = GetCellNeedToRemove();
                       if (lstRemoveCellAfterRain.Count > 0)
                       {                        
                           
                           Score.AddScoreFromRemovedCell(lstRemoveCellAfterRain.Count, true, iChainLevel);
                           RemoveBlocked(lstRemoveCellAfterRain);
                           Delay(300);
                       }
                       else
                       {
                           IsThereAnyCellNeedtoBeRemove = false;
                       }
                       iChainLevel++;
                   }

                   //RemoveBlocked(SwapResult.lstNeedToRemoveForCell2);
                   eRenderType = enRenderType.WholeTable;
                   if (IsEndGame())
                   {
                       _IsGameOver = true;
                       if (GameOverEvent != null)
                       {
                           GameOverEvent();
                       }
                   }
                  // this.RenderTable();
               }
               else
               {
                   eRenderType = enRenderType.WholeTable;

                 //  this.RenderOnlyCell(FirstCell);
                 //  this.RenderOnlyCell(SecondCell);
 
               }
               FirstCell = null;
               SecondCell = null;  

                //if(IsValidPoint (
                
            }
            
        }
        private void RemoveBlocked(List<Point> lst)
        {
            int i;
            eRenderType = enRenderType.WholeTable;
            for (i = 0; i < lst.Count; i++)
            {
                Util.WriteLog("                 Removed cell x:" + lst[i].X.ToString() + " y:" + lst[i].Y.ToString());
                this.PnlGrid.Table.Cell(lst[i]).Value = "";
                this.DelayForRemoveCell();
                //this.eRenderType = enRenderType.RemoveCell;

                this.RenderOnlyCell(this.PnlGrid.Table.Cell(lst[i]));
                

            }
            this.DelayAfterReMovedCell();
            //this.RenderTable();
        }
        private bool IsValidPoint(Point P)
        {
            if (P.X < 0)
                return false;
            if (P.Y < 0)
                return false;

            if (P.X >= this._Col)
                return false;

            if (P.Y >= this._Row)
                return false;

            return true;
        }
    
        private AttemptToMoveBlockResult  AttemptToMoveBloack(TableCell TC, Point Position)
        {
            int i;
            int j;
            List<enDirection> lstDir = new List<enDirection>();
            lstDir.Add(enDirection.Up);
            lstDir.Add(enDirection.Down);
            lstDir.Add(enDirection.Left);            
            lstDir.Add(enDirection.Right);
            Dictionary<enDirection, List<Point>> DicNextToSameValueForEachDir = new Dictionary<enDirection, List<Point>>();

            foreach (enDirection e in lstDir)
            {
                DicNextToSameValueForEachDir.Add(e,new  List<Point>());
            }

            Cell c = TC.Cell(Position);
            string Value = c.Value;
            Point PCheckPoint=new Point (0,0);
            int iSameValueCountInaRow = 0;
            string PreviousValue = "";
            bool CanMove = false;
            List<Point> lstNeedToRemoveBlock = new List<Point>();
            

            for (i = 0; i < lstDir.Count; i++)
            {
                iSameValueCountInaRow = 0;
                PreviousValue = "";
                bool HasMetDifferentCell = false;
                for (j = 1; j <= 2 && !HasMetDifferentCell ; j++)
                {
                    
                    switch (lstDir[i])
                    {
                        case enDirection.Up:
                            PCheckPoint = new Point(c.Position.X, c.Position.Y +j);
                            break;
                        case  enDirection.Down :
                            PCheckPoint = new Point(c.Position.X, c.Position.Y - j);
                            break;
                        case enDirection.Left :
                            PCheckPoint = new Point(c.Position.X + j, c.Position.Y);
                            break;
                        case  enDirection.Right :
                            PCheckPoint = new Point(c.Position.X - j, c.Position.Y);
                            break;
                    }
                    if (!IsValidPoint(PCheckPoint))
                    {
                        continue;
                    }

                    if (TC.Cell(PCheckPoint).Value == Value)
                    {
                        DicNextToSameValueForEachDir[lstDir[i]].Add(new Point(PCheckPoint.X, PCheckPoint.Y));

                        /*
                        if (PreviousValue != Value )
                        {
                            iSameValueCountInaRow = 1;                            
                            PreviousValue = Value;
                        } else                                               
                        {
                            iSameValueCountInaRow++;
                        }
                         */
                    }
                    else
                    {
                        HasMetDifferentCell = true;
                    }                   
                }
                
            }

            if (DicNextToSameValueForEachDir[enDirection.Left].Count + DicNextToSameValueForEachDir[enDirection.Right].Count >= 2)
            {
                lstNeedToRemoveBlock.AddRange(DicNextToSameValueForEachDir[enDirection.Left]);
                lstNeedToRemoveBlock.AddRange(DicNextToSameValueForEachDir[enDirection.Right]);

                CanMove = true;
            }
            
            if (DicNextToSameValueForEachDir[enDirection.Up ].Count + DicNextToSameValueForEachDir[enDirection.Down].Count >= 2)
            {
                lstNeedToRemoveBlock.AddRange(DicNextToSameValueForEachDir[enDirection.Up ]);
                lstNeedToRemoveBlock.AddRange(DicNextToSameValueForEachDir[enDirection.Down ]);
                CanMove = true;
            }
            if (CanMove)
            {
                lstNeedToRemoveBlock.Add(new Point(c.Position.X, c.Position.Y));
            }


            return new AttemptToMoveBlockResult(CanMove, lstNeedToRemoveBlock);
        }
        private void RemoveBlockedIntheSameLine()
        {

        }
        private string CellInvo(Cell c)
        {
            return "    x:" + c.Position.X.ToString() + " y:" + c.Position.Y.ToString() + " value:" + c.Value;
        }
        private List<Point> GetLowestEmptyCells()
        {
            
            
            List<int> lstEmptyColumnX = new List<int>();
            List<Point> lstResultReal = new List<Point>();
            var emptyColumn = this.PnlGrid.Table.Cells.Where(o => o.Value == "").ToList ();
            int i;
            int j;
            for (i = 0; i < emptyColumn.Count; i++)
            {
                bool AlreadyExistinResult = false;
                for (j = 0; j < lstResultReal.Count; j++)
                {
                    if (lstResultReal[j].X == emptyColumn[i].Position.X)
                    {

                        if (lstResultReal[j].X == 1)
                        {
                            string str = "Debug";
                        }
                        if (lstResultReal[j].Y < emptyColumn[i].Position.Y)
                        {
                            lstResultReal[j] = new Point(emptyColumn[i].Position.X,
                                               emptyColumn [i].Position.Y);

                        }
                        AlreadyExistinResult = true;
                    }

                }
                if (!AlreadyExistinResult)
                {
                    lstResultReal.Add(new Point(emptyColumn[i].Position.X, emptyColumn[i].Position.Y));
                }
            }

            return lstResultReal;

        }
        List<CellDecorator> lstMovingCell = new List<CellDecorator>();
        //List<cCellDecorator> lstRainingCell = new List<cCellDecorator>();
        List<CellDecorator> lstStandStillCell = new List<CellDecorator>();
        List<CellDecoratorForRaining> lstRainingCell = new List<CellDecoratorForRaining>();
        private Point GetCellRenderPostion(Cell c)
        {
            Point P = new Point(c.Position.X * this.PnlGrid.CellSize, c.Position.Y * this.PnlGrid.CellSize);
            return P;
        }

        private void RainBlock()
        {
            var lstEmptyCell = GetLowestEmptyCells();
            int i;
            int j;
            lstRainingCell = new List<CellDecoratorForRaining>();
            lstStandStillCell = new List<CellDecorator>();

            Dictionary<int, int> DicNoofEmptyBlockByColumn = new Dictionary<int, int>();
            int LastRowLindex= this.PnlGrid.Table.Rows.Count -1;
            for (i = LastRowLindex ; i >= 0;i--)
                for (j = 0; j < this.PnlGrid.Table.Rows[0].Cols.Count ; j++)
                {
                    Cell c = this.PnlGrid.Table.Rows[i].Cols[j];
                    if (c.Value == "")
                    {
                        if (!DicNoofEmptyBlockByColumn.ContainsKey(c.Position.X))
                        {
                            DicNoofEmptyBlockByColumn[c.Position.X] = 0;
                        }
                        DicNoofEmptyBlockByColumn[c.Position.X]--;

                        Cell cNewCell = new Cell(new Point(c.Position.X, DicNoofEmptyBlockByColumn[c.Position.X ]));
                        cNewCell.Value = GetRandomCellValue().ToString();

                        CellDecoratorForRaining cDecor = new CellDecoratorForRaining(cNewCell, c);
                        cDecor.B = GetBitFromCell(cDecor.Cell);
                        cDecor.GoalPosition = GetCellRenderPostion(c);
                        cDecor.RenderPosition = GetCellRenderPostion(cNewCell);

                        lstRainingCell.Add(cDecor);
                    }
                    else
                    {
                        CellDecorator cDecor = new CellDecorator(c);
                        cDecor.B = GetBitFromCell(cDecor.Cell);
                        lstStandStillCell.Add(cDecor);
                    }
                }
            bool HaveAllCellsReachGoal = false;
            int iPixelMove = 1;
            while (!HaveAllCellsReachGoal)
            {
                if (iPixelMove < 9)
                {
                    iPixelMove++;
                }
                HaveAllCellsReachGoal = true;
                for (i = 0; i < lstRainingCell.Count; i++)
                {
                    CellDecoratorForRaining c = lstRainingCell[i];
                    
                    if (!c.HasReachedGoal)
                    {
                        HaveAllCellsReachGoal = false;
                        c.RenderPosition.Y += iPixelMove;
                        if (c.RenderPosition.Y > c.GoalPosition.Y)
                        {
                            c.RenderPosition.Y = c.GoalPosition.Y;
                            
                        }

                    }                                        
                }
                Delay(20);                
                RenderRainingCell();
            }
            for (i = 0; i < lstRainingCell.Count; i++)
            {
                CellDecoratorForRaining c = lstRainingCell[i];
                c.setOldCellValue();
              

            }            
            lstRainingCell.Clear();
            lstStandStillCell.Clear();
            this.RenderTable();

           
        }
        private void FallBlockV2()
        {
            int i;
            int j;
            int iRow;
            int iCol;
            int iNewCell = 0;
            var lstEmptyCell = GetLowestEmptyCells();
            List<Point> lstColNeedtoFall = new List<Point>();
            bool StillHasBlockToRemove = true;
            
            while (StillHasBlockToRemove)
            {
                StillHasBlockToRemove = false;
                
                for (iRow = this.PnlGrid.Table.Rows.Count - 1; iRow >= 0; iRow--)
                {

                    for (iCol = 0; iCol < this.PnlGrid.Table.Rows[0].Cols.Count; iCol++)
                    {
                        Cell c = this.PnlGrid.Table.Rows[iRow].Cols[iCol];
                        bool IsThisColAlreadyIntheList = false;
                        foreach (Point Ptemp in lstColNeedtoFall)
                        {
                            if (Ptemp.X == iCol)
                            {
                                IsThisColAlreadyIntheList = true;
                                break;
                            }
                        }

                        if (IsThisColAlreadyIntheList)
                        {
                            continue;
                        }
                        if (c.Value == "")
                        {
                            bool HasTheUpperBlockThatHasThevalue = false;
                          
                         
                                for (i = iRow - 1; i >= 0; i--)
                                {
                                    if (this.PnlGrid.Table.Rows[i].Cols[iCol].Value != "")
                                    {
                                        HasTheUpperBlockThatHasThevalue = true;
                                        break;
                                    }
                                }
                            

                            if (HasTheUpperBlockThatHasThevalue  )
                            {
                                lstColNeedtoFall.Add(new Point(c.Position.X, c.Position.Y));
                                
                            }
                        }
                    }

                }
                if (lstColNeedtoFall.Count > 0)
                {
                    StillHasBlockToRemove = true;
                }

                if (StillHasBlockToRemove)
                {
                    for (i = 0; i < this.PnlGrid.Table.Rows.Count; i++)
                    {
                        for (j = 0; j < this.PnlGrid.Table.Rows[i].Cols.Count; j++)
                        {
                            int k;
                            CellDecorator cDecor = new CellDecorator(this.PnlGrid.Table.Rows[i].Cols[j]);
                            cDecor.IsFalling = false;
                            if (cDecor.Cell.Value != "")
                            {
                                cDecor.B = GetBitFromCell(cDecor.Cell);
                            }
                            if (cDecor.Cell.Value != "")
                            {
                                for (k = 0; k < lstColNeedtoFall.Count; k++)
                                {
                                    if (lstColNeedtoFall[k].X == cDecor.Cell.Position.X &&
                                        lstColNeedtoFall[k].Y > cDecor.Cell.Position.Y)
                                    {
                                        cDecor.IsFalling = true;
                                        break;
                                    }
                                }
                            }
                            
                            
                            lstMovingCell.Add(cDecor);
                        }
                    }                 

                    int iPixelMove = 1;
                    int iPixelMoveCount = 0;
                    bool HasFinishedMoving = false;
                    int cellSize = this.PnlGrid.CellSize;
                    /*
                    List<Point> lstNeedToRemoveForCell1 = new List<Point>();
                    List<Point> lstNeedToRemoveForCell2 = new List<Point>();
                    cAttemptToMoveBlockResult AttemptResultFromFirstCell = null;
                    cAttemptToMoveBlockResult AttemptResultFromSecondCell = null;
                     */
                    
                        while (!HasFinishedMoving)
                        {
                            iPixelMove++;
                            if (iPixelMove >= 5)
                            {
                                iPixelMove = 5;
                            }
                            iPixelMoveCount += iPixelMove;
                            if (iPixelMoveCount >= cellSize)
                            {
                                iPixelMoveCount = cellSize;
                                HasFinishedMoving = true;
                            }

                            for (i = 0; i < lstMovingCell.Count; i++)
                            {
                                if (lstMovingCell[i].IsFalling)
                                {
                                    lstMovingCell[i].RenderPosition.Y = lstMovingCell[i].Cell.Position.Y * cellSize + iPixelMoveCount;
                                    lstMovingCell[i].RenderPosition.X = lstMovingCell[i].Cell.Position.X * cellSize;

                                }
                                else
                                {
                                    lstMovingCell[i].RenderPosition.Y = lstMovingCell[i].Cell.Position.Y * cellSize ;
                                    lstMovingCell[i].RenderPosition.X = lstMovingCell[i].Cell.Position.X * cellSize ;

                                }
                            }
                            DelayForFallingCell();
                            RenderFallingCell();
                        }

                        for (i = 0; i < lstColNeedtoFall.Count; i++)
                        {

                            Point P = lstColNeedtoFall[i];
                            for (j = P.Y; j > 0; j--)
                            {                                                                
                                Cell OldCell = this.PnlGrid.Table.Rows[j].Cols[P.X];
                                Cell NewCell = this.PnlGrid.Table.Rows[j -1].Cols[P.X];
                                OldCell.Value = NewCell.Value;
                                NewCell.Value = "";
                                 /*
                                if (OldCell.Value != "")
                                {
                                    cCellDecorator cDecor = new cCellDecorator(OldCell);
                                    lstMovingCell.Add(cDecor);
                                }  
                                  */
                            }
                        }
                    
                    lstColNeedtoFall.Clear();
                    lstMovingCell.Clear();
                }
            }
        }
        private void FallBlock()
        {
            int i;
            int iRow;
            int iNewCell = 0;
            var lstEmptyCell = GetLowestEmptyCells();
            for (i = 0; i < lstEmptyCell.Count; i++)
            {
                Point P = lstEmptyCell[i];
                Util.WriteLog("Col:" + P.X.ToString()); 
                for (iRow = P.Y; iRow > 0; iRow--)
                {
                    Cell cNewCell = null;
                    if (P.X == 1)
                    {
                        string str = "Debug";
                    }
                    for (iNewCell = iRow - 1; iNewCell >= 0; iNewCell--)
                    {
                        if (PnlGrid.Table.Cell(P.X, iNewCell).Value != "")
                        {
                            cNewCell = PnlGrid.Table.Cell(P.X, iNewCell);
                            break;
                        }
                    }

                    if (cNewCell == null)
                    {
                        continue;
                    }
                    string OldValue = PnlGrid.Table.Cell(P.X, iRow).Value;
                    string NewValue = cNewCell.Value;

                    PnlGrid.Table.Cell(P.X, iRow).Value = cNewCell.Value;
                    Util.WriteLog("    Fall :" + CellInvo(PnlGrid.Table.Cell(P.X, iRow)));
                    Util.WriteLog("    Old:" + OldValue + "=>" + NewValue);
                    cNewCell.Value = "";
                    eRenderType = enRenderType.WholeTable;
                    //RenderOnlyCell(PnlGrid.Table.Cell(P.X, iRow));
                    DelayForFallingCell();
                    RenderTable();
                    

                    //iRow = iNewCell;
                }
                PnlGrid.Table.Cell(P.X, 0).Value = "";
                //RenderOnlyCell(PnlGrid.Table.Cell(P.X, 0));
                
                
                RenderTable();

            }

            //RenderTable();
        }
        
        private bool IsBeingSwap = false;
        Point PFirst = new Point(0, 0);
        Point PSecond = new Point(0, 0);
        private TableCell GetCloneTableWithAttemptValue(Cell pFirstCell,Cell pSecondCell)
        {
            TableCell TC = new TableCell();
            TC = PnlGrid.Table.Clone();
            TC.Cell(pFirstCell.Position).Value = pSecondCell.Value;
            TC.Cell(pSecondCell.Position).Value = pFirstCell.Value;
            return TC;
        }

        private TableCell ConvertFromDicToTableCell(Dictionary<Point,string> pDic)
        {
            TableCell TC = new TableCell();
            TC = PnlGrid.Table.CloneStructure();
            foreach (Point P in pDic.Keys)
            {
                TC.Cell(P).Value = pDic[P];
            }
            
            return TC;
        }

        private void SwapCellValue(Point P1, Point P2)
        {
            string strTempValue = PnlGrid.Table.Cell(P1).Value;
            PnlGrid.Table.Cell(P1).Value = PnlGrid.Table.Cell(P2).Value;
            PnlGrid.Table.Cell(P2).Value = strTempValue;

        }
        private int GetRandomCellValue()
        {
            return Util.GetRandom(1, 7);
        }
        private SwapBlockResult  SwapBlock(Cell cFirstCell, Cell cSecondCell)
        {

            IsBeingSwap = true;


            int cellSize = this.PnlGrid.CellSize;
            int i = 0;
            PFirst = new Point(FirstCell.Position.X * this.PnlGrid.CellSize, FirstCell.Position.Y * this.PnlGrid.CellSize);
            PSecond = new Point(SecondCell.Position.X * this.PnlGrid.CellSize, SecondCell.Position.Y * this.PnlGrid.CellSize);
            Point PFirstOriginal = new Point(PFirst.X, PFirst.Y);
            Point PSecondOriginal = new Point(PSecond.X, PSecond.Y);

            enDirection Dir = GetMoveDirection();
            int iPixelMove = 3;
            int iPixelMoveCount = 0;
            bool HasFinishedMoving = false;
            bool IsNeedtoSwapBack = false;
            List<Point> lstNeedToRemoveForCell1 = new List<Point>();
            List<Point> lstNeedToRemoveForCell2 = new List<Point>();
            AttemptToMoveBlockResult AttemptResultFromFirstCell = null;
            AttemptToMoveBlockResult AttemptResultFromSecondCell = null;
            while (!HasFinishedMoving )
            {
                iPixelMoveCount += iPixelMove;
                if (iPixelMoveCount >= cellSize)
                {
                    iPixelMoveCount = cellSize;
                    HasFinishedMoving = true;

                    TableCell TC=new TableCell ();
                    TC = GetCloneTableWithAttemptValue(FirstCell, SecondCell);
                    AttemptResultFromFirstCell = AttemptToMoveBloack(TC, FirstCell.Position);
                    AttemptResultFromSecondCell = AttemptToMoveBloack(TC, SecondCell.Position );

                    if (!AttemptResultFromFirstCell.CanMove &&
                        !AttemptResultFromSecondCell.CanMove)
                    {
                        IsNeedtoSwapBack = true;
                    }
                    else
                    {
                        SwapCellValue(cFirstCell.Position, cSecondCell.Position);
                        Util.WriteLog("   SwapCellValue");
                    }
                    Util.WriteLog("   Has finished moving");
                    
                }              
                         
                    switch (Dir)
                    {
                        case enDirection.Up:
                            PFirst.Y = PFirstOriginal.Y - iPixelMoveCount;
                            PSecond.Y = PSecondOriginal.Y + iPixelMoveCount;

                            break;
                        case enDirection.Down:
                            PFirst.Y = PFirstOriginal.Y + iPixelMoveCount;
                            PSecond.Y = PSecondOriginal.Y - iPixelMoveCount;
                            break;
                        case enDirection.Left:
                            PFirst.X = PFirstOriginal.X - iPixelMoveCount;
                            PSecond.X = PSecondOriginal.X + iPixelMoveCount;

                            break;
                        case enDirection.Right:
                            PFirst.X = PFirstOriginal.X + iPixelMoveCount;
                            PSecond.X = PSecondOriginal.X - iPixelMoveCount;
                            break;
                    }
                    if (!HasFinishedMoving)
                    {
                        this.DelayForMoveCell();
                        RenderFirstAndSecondCell();
                    }
                
            }
            if (IsNeedtoSwapBack)
            {
                DelayBecauseMovedIncorrectCell();
                iPixelMoveCount = 0;
                HasFinishedMoving = false;
                
              
                PFirstOriginal = new Point(PFirst.X, PFirst.Y);
                PSecondOriginal = new Point(PSecond.X, PSecond.Y);

                while (!HasFinishedMoving)
                {
                    iPixelMoveCount += iPixelMove;
                    if (iPixelMoveCount >= cellSize)
                    {
                        iPixelMoveCount = cellSize;
                        HasFinishedMoving = true;                     
                    }
                    switch (Dir)
                    {
                        case enDirection.Up:
                            PFirst.Y = PFirstOriginal.Y + iPixelMoveCount;
                            PSecond.Y = PSecondOriginal.Y - iPixelMoveCount;

                            break;
                        case enDirection.Down:
                            PFirst.Y = PFirstOriginal.Y - iPixelMoveCount;
                            PSecond.Y = PSecondOriginal.Y + iPixelMoveCount;
                            break;
                        case enDirection.Left:
                            PFirst.X = PFirstOriginal.X + iPixelMoveCount;
                            PSecond.X = PSecondOriginal.X - iPixelMoveCount;

                            break;
                        case enDirection.Right:
                            PFirst.X = PFirstOriginal.X - iPixelMoveCount;
                            PSecond.X = PSecondOriginal.X + iPixelMoveCount;
                            break;
                    }

                    this.DelayForMoveCell();
                    RenderFirstAndSecondCell();
                }
            }

            IsBeingSwap = false;
            SwapBlockResult cResult = new SwapBlockResult(!IsNeedtoSwapBack, AttemptResultFromFirstCell.lstNeedToRemove, AttemptResultFromSecondCell.lstNeedToRemove);

            return cResult;
        }
        
        private Dictionary<Point, string> _MyDic = null;
        private string MyDicPath = Util.CurrentPath + @"\MyDic.bin";
        private Dictionary<Point, string> MyDic
        {
            get
            {
                if (_MyDic == null)
                {
                    _MyDic = (Dictionary<Point, string>)Util.ReadObj(MyDicPath);
                }
                return _MyDic;
            }
        }
        
        public void GenerateBoard()
        {
            int i;
            int j;
            
            //Dictionary<Point, string> Dic = new Dictionary<Point, string>();
            _MyDic = new Dictionary<Point, string>();
            for (i = 0; i < this.PnlGrid.Table.Rows.Count; i++)
            {
                for (j = 0; j < this.PnlGrid.Table.Rows[0].Cols.Count; j++)
                {
                    Cell c = this.PnlGrid.Table.Rows[i].Cols[j];
                    c.Value = GetRandomCellValue().ToString ();
                    _MyDic.Add(c.Position, c.Value);
                }
            }
            bool IsValidBoard = false;
            while (!IsValidBoard)
            {
                List<Point> lstRemoveCellAfterGenBoard = GetCellNeedToRemoveFromDic(_MyDic );
                if (lstRemoveCellAfterGenBoard.Count == 0)
                {
                    if (!IsEndGame(_MyDic ))
                    {
                        IsValidBoard = true;
                    }
                }
                if(!IsValidBoard)
                {
                    foreach (Point P in lstRemoveCellAfterGenBoard)
                    {
                        _MyDic [P] = GetRandomCellValue().ToString();
                    }
                }
            }
                     
            
        
        }

        public void GenerateBoardV2()
        {
            int i;
            int j;
            Dictionary<Point, string> Dic = new Dictionary<Point, string>();
            int iTemp = 0;
            for (i = 0; i < this.PnlGrid.Table.Rows.Count; i++)
            {
                for (j = 0; j < this.PnlGrid.Table.Rows[0].Cols.Count; j++)
                {
                    iTemp++;
                    if (iTemp > 6)
                    {
                        iTemp = 1;
                    }
                    Cell c = this.PnlGrid.Table.Rows[i].Cols[j];
                    c.Value = iTemp.ToString();
                    Dic.Add(c.Position, c.Value);
                }
            }
            

            Util.WriteObj(Dic, MyDicPath);
        }
        public void EndGameFromTimeOut()
        {
            _IsGameOver = true;
            if (GameOverEvent != null)
            {
                GameOverEvent();
            }

        }
        public void SaveCurrentBoard()
        {

            int i;
            int j;
            Dictionary<Point, string> Dic = new Dictionary<Point, string>();
            for (i = 0; i < this.PnlGrid.Table.Rows.Count; i++)
            {
                for (j = 0; j < this.PnlGrid.Table.Rows[0].Cols.Count; j++)
                {
                    Cell c = this.PnlGrid.Table.Rows[i].Cols[j];
                    Dic.Add(c.Position, c.Value);
                }
            }
            Util.WriteObj(Dic, MyDicPath );
        }
        private void InitialState()
        {
            /*
            this.LoadNewBlockIntolstNextBlock();
            this.GetCurrentBlockFromQuery();
            */
           // this.PnlGrid.PanelPaintEvent+=new cPanelGrid.PanelPainEventHandler(PnlGrid_PanelPaintEvent);            
            this.PnlGrid.IsCustomRendering = true;
            this.PnlGrid.IsRenderingOntime = false;
            this.PnlGrid.PanelPaintEvent -= new PanelGrid.PanelPainEventHandler(cP_PanelPaintEvent);
            this.PnlGrid.CellClickEvent -= new PanelGrid.CellClickEventHandler(cP_CellClickEvent);
            this.PnlGrid.PanelPaintEvent += new PanelGrid.PanelPainEventHandler(cP_PanelPaintEvent);
            this.PnlGrid.CellClickEvent += new PanelGrid.CellClickEventHandler(cP_CellClickEvent);

            int i;
            int j;
            StringBuilder strB = new StringBuilder();

            this.GenerateBoard();


            for (i = 0; i < this.PnlGrid.NumberofRows; i++)
            {

                for (j = 0; j < this.PnlGrid.NumberofCols; j++)
                {
                  
                    Point P = new Point(j, i);
                    this.PnlGrid.Table.Cell(j, i).Value = MyDic[P];                
                }

            }

            _Score = new Score();
            _Score.ClearScore();
            _Score.ScoreChangeEvent+=new Score.ScoreChangeEventHandler(_ScoreChangeEvent);
            
            RenderNewBackGround();
            RenderTable();



        }
        public void _ScoreChangeEvent(int NewScore)
        {
            if (ScoreChangeEvent != null)
            {
                ScoreChangeEvent(NewScore);
            }
        }
        /*
        private void RenderSwapingCell(Graphics g)
        {

            string strFirstCellImage = FirstCell.ImageFilePath;
            string strSecondCellImage = SecondCell.ImageFilePath;
            Bitmap B = new Bitmap(strFirstCellImage );
            Bitmap B2 = new Bitmap(strSecondCellImage );
            

            Bitmap BResized = new Bitmap(B, new Size(this.PnlGrid.CellSize , this.PnlGrid.CellSize));
           
            //B.Dispose();
            //B2.Dispose();

            //g.Clear(Color.Transparent);
            System.Drawing.Brush Br = new SolidBrush(Color.Black);
            //System.Drawing.Pen P = new Pen(Br);
            //System.Drawing.Rectangle R = new Rectangle(, 0, BResized.Width, BResized.Height);
            //g.DrawRectangle(P, R);
            //g.FillRectangle(Br, R);
            int i;
            int j;
        }
         * */

        public SwipPicGame( int pRow, int pCol)
        {
            _Pnl = CreateDoubleBufferedPanel();
         
            _Row = pRow;
            _Col = pCol;
            _PnlGrid = new PanelGrid(_Pnl, pRow, pCol, 43);
            

            //_PnlGrid.CellClickEvent += new cPanelGrid.CellClickEventHandler(PnlGrid_CellClickEvent);
            //_Score = 0;
            this.InitialState();
        }
    }
}
