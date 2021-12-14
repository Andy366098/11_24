using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringBehavior
{
    static public bool Seek(AIData data)
    {
        Vector3 currentPos = data.m_Go.transform.position;
        Vector3 vec = data.m_vTarget - currentPos;
        vec.y = 0.0f;   //�N�V�q��Y�ܶq�o��
        float fDist = vec.magnitude;
        if(fDist < data.m_fArriveRange)   //�p�G�w�g�ܱ���ؼ��I�F
        {
            //�N�⥦��l�ơA�è�F�ؼ��I
            Vector3 vFinal = data.m_vTarget;
            vFinal.y = currentPos.y;
            data.m_Go.transform.position = vFinal;
            data.m_fMoveForce = 0.0f;
            data.m_fTempTurnForce = 0.0f;
            data.m_Speed = 0.0f;
            data.m_bMove = false;
            return false;
        }
        Vector3 vf = data.m_Go.transform.forward;   //���쪫�骺forward�V�q
        Vector3 vr = data.m_Go.transform.right;
        data.m_vCurrentVector = vf;
        vec.Normalize();
        float fDotF = Vector3.Dot(vf, vec); //forward�V�q�򩹥ؼЪ��V�q���n
        if(fDotF > 0.96f)   //�p�G�ؼЦV�q�w�g��G���歱�V��V
        {
            fDotF = 1.0f;   //�N��@�L�w�g���V�ؼ�
            data.m_vCurrentVector = vec;
            data.m_fTempTurnForce = 0.0f;
            data.m_fRot = 0.0f;
        }
        else
        {
            if (fDotF < -1.0f)  //�p�G���n�X�Ӧ]�B�I�ƻ~�t�Ӥp��-1.0f
            {
                fDotF = -1.0f;  //�N��������-1.0f
            }
            float fDotR = Vector3.Dot(vr, vec);     //�ؼЦV�q�PRight�b�����n�A��ܧ�v�bright���
            if (fDotF < 0.0f)   //�p�G�ӥؼ��I�b����
            {
                if (fDotR > 0.0f)   //�B�b�k���
                {
                    fDotR = 1.0f;   //���k�j��
                }
                else
                {                   //�b�����
                    fDotR = -1.0f;  //�����j��
                }
            }
            if(fDist < 3.0f)        //�p�G�Z���w�g�ܪ�
            {
                fDotR *= ((1.0f - fDist / 3.0f) + 1.0f);    //��V�O�W�[ ��K���ؼ��I
            }
            data.m_fTempTurnForce = fDotR;  //�s�i���
        }
        if (fDist < data.m_fDeSpeedRange)   //�p�G�w��F��t�b�|��
        {
            if(data.m_Speed > 5.0f)     //�B���@�w�t��
            {
                data.m_fMoveForce = -(1.0f - fDist / data.m_fDeSpeedRange) * Time.deltaTime * 10.0f; //�Ϩ��t
            }
            else
            {
                data.m_fMoveForce = fDotF;  //��t
            }
        }
        else
        {
            data.m_fMoveForce = fDotF;//��t
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
            Vector3 vf = data.m_vCurrentVector;   //���쪫�骺forward�V�q
            Vector3 vr = data.m_Go.transform.right;
            vf = vf + vr * data.m_fTempTurnForce;
            vf.Normalize();
            data.m_Go.transform.forward = vf;
            //�٨S�[�̤j��V����
            //�S�[�I������
            //����̤p��̤j�t��
            data.m_Speed = data.m_Speed + data.m_fMoveForce;
            if (data.m_Speed < 0.1f)
            {
                data.m_Speed = 0.1f;
            }
            else if (data.m_Speed > data.m_fMaxSpeed)
            {
                data.m_Speed = data.m_fMaxSpeed;
            }
            //�}�l����
            currentPos = currentPos + vf * data.m_Speed * Time.deltaTime;   //�h���FDeltaTime���L�C�I
            data.m_Go.transform.position = currentPos;
        }
        
    }
    static public bool CollisionAvoid(AIData data)
    {
        //�Q��Main�޲z����o��ê����T
        List<Obstacle> m_AvoidTargets = Main.m_Instance.GetObstacles();
        //���N�`�n�ާ@�����󮳥X��
        Transform ct = data.m_Go.transform;
        Vector3 cPos = ct.position;
        Vector3 cForward = ct.forward;
        return true;
    }
}
