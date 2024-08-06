using BookStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStore.Helpers
{
    public class StatusHelper
    {
        public static void CheckStatus()
        {
            using (ApplicationDbContext core = new ApplicationDbContext())
            {
                var CheckStatusList = core.Statuses.ToList();
                var CheckRNRD = core.RNRDatas.ToList();
                var CheckReasonDrop = core.ReasonDrops.ToList();

                #region Status check
                var CheckApp = CheckStatusList.FirstOrDefault(x => x.Key == Keys.STS_Approved) ?? null;
                if (CheckApp == null)
                {
                    AddStatus(core, Keys.STS_Approved, "Approved", "Approved");
                }
                var CheckDec = CheckStatusList.FirstOrDefault(x => x.Key == Keys.STS_Declined) ?? null;
                if (CheckApp == null)
                {
                    AddStatus(core, Keys.STS_Declined, "Declined", "Declined");
                }


                var CheckPending = CheckStatusList.FirstOrDefault(x => x.Key == Keys.STS_Pending) ?? null;
                if (CheckPending == null)
                {
                    AddStatus(core, Keys.STS_Pending, "Pending", "Pending");
                }

                #endregion



                #region RNRData
                var CheckRNRfund = CheckRNRD.FirstOrDefault(x => x.Key == Keys.RNR_Refund) ?? null;
                if (CheckRNRfund == null)
                {
                    AddRNRD(core, Keys.RNR_Refund, "Refund", "Refund", "RNR-decision");
                }
                var CheckRNRReturn = CheckRNRD.FirstOrDefault(x => x.Key == Keys.RNR_return) ?? null;
                if (CheckRNRfund == null)
                {
                    AddRNRD(core, Keys.RNR_return, "Return", "Return", "RNR-decision");
                }


                var CheckRNRRefund = CheckRNRD.FirstOrDefault(x => x.Key == Keys.RNRE_Refunded) ?? null;
                if (CheckRNRRefund == null)
                {
                    AddRNRD(core, Keys.RNRE_Refunded, "Refunded", "Refunded", "RNRE-001");
                }
                var CheckRNRReplacement = CheckRNRD.FirstOrDefault(x => x.Key == Keys.RNRE_ReplacementPending) ?? null;
                if (CheckRNRReplacement == null)
                {
                    AddRNRD(core, Keys.RNRE_ReplacementPending, "Will Be Replaced", "Will Be Replaced", "RNRE-001");
                }
                var CheckRNRNoChanged = CheckRNRD.FirstOrDefault(x => x.Key == Keys.RNRE_ReturnGoodsNoChange) ?? null;
                if (CheckRNRNoChanged == null)
                {
                    AddRNRD(core, Keys.RNRE_ReturnGoodsNoChange, "No Futher Action , Orginal Goods Return to Customer", "No Futher Action , Orginal Goods Return to Customer", "RNRE-001");
                }




                var CkcRNRSerHigh = CheckRNRD.FirstOrDefault(x => x.Key == Keys.RNRE_High) ?? null;
                if (CkcRNRSerHigh == null)
                {
                    AddRNRD(core, Keys.RNRE_High, "High", "High", "RNRE-002");
                }
                var CkcRNRSerMedium = CheckRNRD.FirstOrDefault(x => x.Key == Keys.RNRE_Medium) ?? null;
                if (CkcRNRSerMedium == null)
                {
                    AddRNRD(core, Keys.RNRE_Medium, "Medium", "Medium", "RNRE-002");
                }
                var CkcRNRSerLow = CheckRNRD.FirstOrDefault(x => x.Key == Keys.RNRE_Low) ?? null;
                if (CkcRNRSerLow == null)
                {
                    AddRNRD(core, Keys.RNRE_Low, "Low", "Low", "RNRE-002");
                }



                #endregion




                #region Reasons List
                var CheckReason = CheckReasonDrop.FirstOrDefault(x => x.Key == Keys.Reason_Damaged) ?? null;
                if (CheckReason == null)
                {
                    AddReasons(core, Keys.Reason_Damaged, "Damaged", "Damaged");
                }
                var CheckReason2 = CheckReasonDrop.FirstOrDefault(x => x.Key == Keys.Reason_IncorrectItem) ?? null;
                if (CheckReason == null)
                {
                    AddReasons(core, Keys.Reason_IncorrectItem, "Incorrect Item", "Incorrect Item");
                }
                var CheckReason3 = CheckReasonDrop.FirstOrDefault(x => x.Key == Keys.Reason_Other) ?? null;
                if (CheckReason == null)
                {
                    AddReasons(core, Keys.Reason_Other, "Other", "Other");
                }
                #endregion

            }
        }

        public static void AddStatus(ApplicationDbContext core, string Key, string Description, string name)
        {
            Status status = new Status()
            {
                IsActive = true,
                Key = Key,
                Description = Description,
                Name = name,


            };
            core.Statuses.Add(status);
            core.SaveChanges();
        }

        public static void AddRNRD(ApplicationDbContext core, string Key, string Description, string name, string codekey)
        {
            RNRData status = new RNRData()
            {
                IsActive = true,
                Key = Key,
                Description = Description,
                Name = name,
                CodeKey = codekey

            };
            core.RNRDatas.Add(status);
            core.SaveChanges();
        }


        public static void AddReasons(ApplicationDbContext core, string Key, string Description, string name)
        {
            ReasonDrop status = new ReasonDrop()
            {
                IsActive = true,
                Key = Key,
                Description = Description,
                Name = name

            };
            core.ReasonDrops.Add(status);
            core.SaveChanges();
        }

        public static Status GetStatus(string Key)
        {
            using (ApplicationDbContext core = new ApplicationDbContext())
            {
                var CheckPending = core.Statuses.FirstOrDefault(x => x.Key == Key) ?? null;
                return CheckPending;
            }

        }


        public static RNRData GetRNRDate(string Key)
        {
            using (ApplicationDbContext core = new ApplicationDbContext())
            {
                try
                {
                    return core.RNRDatas.FirstOrDefault(x => x.Key == Key) ?? null;
                }
                catch (Exception)
                {

                    return null;
                }

            }

        }

    }
}