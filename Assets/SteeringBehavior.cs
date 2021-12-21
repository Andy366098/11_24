using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringBehavior
{
    static public bool Seek(AIData data)
    {
        Vector3 currentPos = data.m_Go.transform.position;
        Vector3 vec = data.m_vTarget - currentPos;
        vec.y = 0.0f;   //將向量的Y變量濾掉
        float fDist = vec.magnitude;
        if(fDist < data.m_fArriveRange)   //如果已經很接近目標點了
        {
            //就把它初始化，並到達目標點
            Vector3 vFinal = data.m_vTarget;
            vFinal.y = currentPos.y;
            data.m_Go.transform.position = vFinal;
            data.m_fMoveForce = 0.0f;
            data.m_fTempTurnForce = 0.0f;
            data.m_Speed = 0.0f;
            data.m_bMove = false;
            return false;
        }
        Vector3 vf = data.m_Go.transform.forward;   //拿到物體的forward向量
        Vector3 vr = data.m_Go.transform.right;
        data.m_vCurrentVector = vf;
        vec.Normalize();
        float fDotF = Vector3.Dot(vf, vec); //forward向量跟往目標的向量內積
        if(fDotF > 0.96f)   //如果目標向量已經近乎平行面向方向
        {
            fDotF = 1.0f;   //就當作他已經面向目標
            data.m_vCurrentVector = vec;
            data.m_fTempTurnForce = 0.0f;
            data.m_fRot = 0.0f;
        }
        else
        {
            if (fDotF < -1.0f)  //如果內積出來因浮點數誤差而小於-1.0f
            {
                fDotF = -1.0f;  //就讓它等於-1.0f
            }
            float fDotR = Vector3.Dot(vr, vec);     //目標向量與Right軸的內積，表示投影在right比例
            if (fDotF < 0.0f)   //如果該目標點在身後
            {
                if (fDotR > 0.0f)   //且在右後方
                {
                    fDotR = 1.0f;   //往右大轉
                }
                else
                {                   //在左後方
                    fDotR = -1.0f;  //往左大轉
                }
            }
            if(fDist < 3.0f)        //如果距離已經很近
            {
                fDotR *= ((1.0f - fDist / 3.0f) + 1.0f);    //轉向力增加 方便轉到目標點
            }
            data.m_fTempTurnForce = fDotR;  //存進資料
        }
        if (fDist < data.m_fDeSpeedRange)   //如果已到達減速半徑裡
        {
            if(data.m_Speed > 5.0f)     //且有一定速度
            {
                data.m_fMoveForce = -(1.0f - fDist / data.m_fDeSpeedRange) * Time.deltaTime * 10.0f; //使其減速
            }
            else
            {
                data.m_fMoveForce = fDotF;  //原速
            }
        }
        else
        {
            data.m_fMoveForce = fDotF;//原速
        }

        data.m_bMove = true;
        //Move
        /*float fAcc = data.m_fMoveForce * data.m_fTempTurnForce;
        float fAcc2 = fDist / data.m_fArriveRange;
        if (fAcc2 > 1.0f)
            fAcc2 = 1.0f;
        else
            fAcc2 = -(1.0f - fAcc2);*/
        Vector3 vOriF = vf;
        

        return true;
    }
    static public void Move(AIData data)
    {
        if (data.m_bMove)
        {
            Vector3 currentPos = data.m_Go.transform.position;
            Vector3 vf = data.m_vCurrentVector;   //拿到物體的forward向量
            Vector3 vr = data.m_Go.transform.right;
            vf = vf + vr * data.m_fTempTurnForce;
            vf.Normalize();
            data.m_Go.transform.forward = vf;
            //還沒加最大轉向控制
            //沒加碰撞偵測
            //控制最小跟最大速度
            data.m_Speed = data.m_Speed + data.m_fMoveForce;
            if (data.m_Speed < 0.1f)
            {
                data.m_Speed = 0.1f;
            }
            else if (data.m_Speed > data.m_fMaxSpeed)
            {
                data.m_Speed = data.m_fMaxSpeed;
            }
            //開始移動
            currentPos = currentPos + vf * data.m_Speed * Time.deltaTime;   //多乘了DeltaTime讓他慢點
            data.m_Go.transform.position = currentPos;
        }
        
    }
    static bool CheckCollision(AIData data)
    {
        //利用Main管理器獲得障礙物資訊
        List<Obstacle> m_AvoidTargets = Main.m_Instance.GetObstacles();
        if (m_AvoidTargets == null)
        {
            return false;
        }
        //先將常要操作的物件拿出來
        Transform ct = data.m_Go.transform;
        Vector3 cPos = ct.position;
        Vector3 cForward = ct.forward;
        data.m_vCurrentVector = cForward;
        //
        Vector3 vec;
        Vector3 vFinalVec = Vector3.forward;
        float fDist = 0.0f;
        float fDot = 0.0f;
        int iCount = m_AvoidTargets.Count;
        for (int i = 0; i < iCount; i++)
        {
            //獲得往迴避目標的向量
            vec = m_AvoidTargets[i].transform.position - cPos;
            vec.y = 0.0f;   //不考慮Y變量
            fDist = vec.magnitude;  //獲得距離
            if (fDist > data.m_fProbeLength + m_AvoidTargets[i].m_fRadius) //距離如果超出探針跟物體半徑
            {
                //將該障礙物的狀態設為Outside，只影響其Gizimo的顏色
                m_AvoidTargets[i].m_eState = Obstacle.eState.OUTSIDE_TEST;
                continue;
            }

            vec.Normalize();    //正規化
            fDot = Vector3.Dot(vec, cForward); //取得往目標向量在目前面相的投影
            if (fDot < 0)   //內積小於0表示目標在身後
            {
                m_AvoidTargets[i].m_eState = Obstacle.eState.OUTSIDE_TEST;
                continue;
            }
            else if (fDot > 1.0f)  //若因誤差超出1.0f
            {
                fDot = 1.0f;
            }
            m_AvoidTargets[i].m_eState = Obstacle.eState.INSIDE_TEST;
            float fProjDist = fDist * fDot; //把它長度乘回去
            //目標向量跟forward向量夾角的那個邊，也就是斜邊平方減一邊的平方
            float fDotDist = Mathf.Sqrt(fDist * fDist - fProjDist * fProjDist);
            if (fDotDist > m_AvoidTargets[i].m_fRadius + data.m_fRadius)    //如果沒有撞到
            {
                continue;
            }
            return true;    //上述條件都沒過表示會撞到
        }
        return false;
    }
    static bool CollisionAvoid(AIData data)
    {
        //利用Main管理器獲得障礙物資訊
        List<Obstacle> m_AvoidTargets = Main.m_Instance.GetObstacles();
        //先將常要操作的物件拿出來
        Transform ct = data.m_Go.transform;
        Vector3 cPos = ct.position;
        Vector3 cForward = ct.forward;
        data.m_vCurrentVector = cForward;
        //
        Vector3 vec;
        Vector3 vFinalVec = Vector3.forward;
        Obstacle oFinal = null;
        float fDist = 0.0f;
        float fDot = 0.0f;
        float fFinalDot = 0.0f;
        int iCount = m_AvoidTargets.Count;

        float fMinDist = 10000.0f;
        for (int i = 0; i < iCount; i++)
        {
            //獲得往迴避目標的向量
            vec = m_AvoidTargets[i].transform.position - cPos;
            vec.y = 0.0f;   //不考慮Y變量
            fDist = vec.magnitude;  //獲得距離
            if (fDist > data.m_fProbeLength + m_AvoidTargets[i].m_fRadius) //距離如果超出探針跟物體半徑
            {
                //將該障礙物的狀態設為Outside，只影響其Gizimo的顏色
                m_AvoidTargets[i].m_eState = Obstacle.eState.OUTSIDE_TEST;  
                continue;
            }

            vec.Normalize();    //正規化
            fDot = Vector3.Dot(vec, cForward); //取得往目標向量在目前面相的投影
            if (fDot < 0)   //內積小於0表示目標在身後
            {
                m_AvoidTargets[i].m_eState = Obstacle.eState.OUTSIDE_TEST;
                continue;
            }else if (fDot > 1.0f)  //若因誤差超出1.0f
            {
                fDot = 1.0f;
            }
            m_AvoidTargets[i].m_eState = Obstacle.eState.INSIDE_TEST;
            float fProjDist = fDist * fDot; //把它長度乘回去
            //目標向量跟forward向量夾角的那個邊，也就是斜邊平方減一邊的平方
            float fDotDist = Mathf.Sqrt(fDist * fDist - fProjDist * fProjDist);
            if (fDotDist > m_AvoidTargets[i].m_fRadius + data.m_fRadius)    //如果沒有撞到
            {
                continue;
            }
            if (fDist < fMinDist)   //將最近的障礙物資訊存起來
            {
                fMinDist = fDist;
                vFinalVec = vec;
                oFinal = m_AvoidTargets[i];
                fFinalDot = fDot;
            }
        }
        if (oFinal != null) //防呆，避免忘了加障礙物閃退
        {
            Vector3 vCross = Vector3.Cross(cForward, vFinalVec);//換一個方式，用外積來算要左轉還右轉
            float fTurnMag = Mathf.Sqrt(1.0f - fFinalDot * fFinalDot);  //因為finalDot已經單位化，因此用1-FinalDot也可得知對邊的長度
            if (vCross.y > 0.0f)    //如果是正的代表要向左轉
            {
                fTurnMag = -fTurnMag;
            }
            data.m_fTempTurnForce = fTurnMag;

            float fTotalLen = data.m_fProbeLength + oFinal.m_fRadius;
            float fRatio = fMinDist / fTotalLen;//力的比率，探針跟物體半徑長的和分之距離
            if (fRatio > 1.0f)   //防呆，比率為0~1的數值
            {
                fRatio = 1.0f;
            }
            fRatio = 1.0f - fRatio; //越靠近力的比率要越大
            data.m_fMoveForce = -fRatio;    //永遠為斥力
            oFinal.m_eState = Obstacle.eState.COL_TEST; //變物體的狀態以讓他變顏色
            data.m_bCol = true;
            data.m_bMove = true;
            return true;
        }
        data.m_bCol = false;
        return false;
    }
}
