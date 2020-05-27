using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.UI.Xaml.Controls;

namespace TheManXS.ViewModel.MapBoardVM.SKGraphics.Infrastructure
{
    class SearchModifier
    {
        public int Row { get; set; }
        public int Col { get; set; }
        public bool AreAdditionalSegmentTypesToAdd { get; set; }
        public SegmentType AdditionalSegmentTypeToAdd { get; set; }
        public bool IsWaterOnly { get; set; }
    }
    class SearchModifierList : List<SearchModifier>
    {
        public SearchModifierList()
        {
            InitList();
        }
        public SearchModifier this[SegmentType segmentType] => this[(int)segmentType];
        private void InitList()
        {
            for (int i = 0; i < (int)SegmentType.TotalAdjSqSegments; i++)
            {
                Add(getSearchModifier((SegmentType)i));
            }

            SearchModifier getSearchModifier(SegmentType segmentType)
            {
                switch (segmentType)
                {
                    case SegmentType.NW_out_to_W:
                        return new SearchModifier()
                        {
                            Row = 0,
                            Col = -1,
                            AreAdditionalSegmentTypesToAdd = false,
                            IsWaterOnly = true
                        };

                    case SegmentType.NW_out_to_N:
                        return new SearchModifier()
                        {
                            Row = -1,
                            Col = 0,
                            AreAdditionalSegmentTypesToAdd = true,
                            AdditionalSegmentTypeToAdd = SegmentType.SWxNW,
                            IsWaterOnly = true,
                        };

                    case SegmentType.NE_out_to_N:
                        return new SearchModifier()
                        {
                            Row = -1,
                            Col = 0,
                            AreAdditionalSegmentTypesToAdd = true,
                            AdditionalSegmentTypeToAdd = SegmentType.NExSE,
                            IsWaterOnly = false,
                        };

                    case SegmentType.NE_out_to_E:
                        return new SearchModifier()
                        {
                            Row = 0,
                            Col = 1,
                            AreAdditionalSegmentTypesToAdd = true,
                            AdditionalSegmentTypeToAdd = SegmentType.NExSE,
                            IsWaterOnly = true,
                        };

                    case SegmentType.SW_out_to_S:
                        return new SearchModifier()
                        {
                            Row = 1,
                            Col = 0,
                            AreAdditionalSegmentTypesToAdd = true,
                            AdditionalSegmentTypeToAdd = SegmentType.SExSW,
                            IsWaterOnly = false,
                        };

                    case SegmentType.SW_out_to_W:
                        return new SearchModifier()
                        {
                            Row = 0,
                            Col = -1,
                            AreAdditionalSegmentTypesToAdd = true,
                            AdditionalSegmentTypeToAdd = SegmentType.SExSW,
                            IsWaterOnly = false,
                        };

                    case SegmentType.SE_out_to_E:
                        return new SearchModifier()
                        {
                            Row = 0,
                            Col = 1,
                            AreAdditionalSegmentTypesToAdd = false,
                            IsWaterOnly = false,
                        };

                    case SegmentType.SE_out_to_S:
                        return new SearchModifier()
                        {
                            Row = 1,
                            Col = 0,
                            AreAdditionalSegmentTypesToAdd = false,
                            IsWaterOnly = false,
                        };

                    case SegmentType.TotalAdjSqSegments:
                    case SegmentType.NWxNE:
                    case SegmentType.NExSE:
                    case SegmentType.SWxNW:
                    case SegmentType.SExSW:
                    case SegmentType.Total:
                    default:
                        return new SearchModifier();
                }
            }
        }
    }
}
