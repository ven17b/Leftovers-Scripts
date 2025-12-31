using System;
using UnityEngine;

namespace Leftovers.Neighbour
{
	public class Bully : MonoBehaviour
	{
		private void OnTriggerEnter(Collider other)
		{
            Leftovers_Neighbour_Bully_o * this,
        UnityEngine_Collider_o* other,
        const MethodInfo* method)
{
                UnityEngine_GameObject_o* gameObject;
                UnityEngine_CharacterController_o* playerController;

                static bool initialized = false;
                if (!initialized)
                {
                    sub_180157A00(&Method_UnityEngine_GameObject_GetComponent_CharacterController___);
                    sub_180157A00(&UnityEngine_Object_TypeInfo);
                    initialized = true;
                }

                if (!other || !(gameObject = UnityEngine_Component__get_gameObject((UnityEngine_Component_o*)other, 0)))
                    sub_180157B40(this);

                playerController = (UnityEngine_CharacterController_o*)UnityEngine_GameObject__GetComponent_Toggle_(
                    gameObject,
                    Method_UnityEngine_GameObject_GetComponent_CharacterController___
                );

                this->fields.playerController = playerController;
                sub_180157590(&this->fields.playerController, playerController);

                if ((UnityEngine_Object_TypeInfo->_2.bitflags2 & 2) != 0 && !UnityEngine_Object_TypeInfo->_2.cctor_finished)
                    il2cpp_runtime_class_init(UnityEngine_Object_TypeInfo, 0);

                if (UnityEngine_Object__op_Implicit((UnityEngine_Object_o*)this->fields.playerController, 0))
                    this->fields.currentForce = this->fields.pushForce;
            }

		private void Update()
		{
            UnityEngine_Object_o* playerController;
            float currentForce;
            UnityEngine_CharacterController_o* charController;
            UnityEngine_Vector3_o moveVector;
            float deltaTime;
            float decaySpeed;

            static bool initialized = false;
            if (!initialized)
            {
                sub_180157A00(&UnityEngine_Object_TypeInfo);
                initialized = true;
            }

            playerController = (UnityEngine_Object_o*)this->fields.playerController;

            if ((UnityEngine_Object_TypeInfo->_2.bitflags2 & 2) != 0 && !UnityEngine_Object_TypeInfo->_2.cctor_finished)
                il2cpp_runtime_class_init(UnityEngine_Object_TypeInfo, method);

            if (UnityEngine_Object__op_Implicit(playerController, 0))
            {
                currentForce = this->fields.currentForce;
                if (currentForce > 0.0)
                {
                    charController = this->fields.playerController;
                    moveVector.fields.x = this->fields.direction.fields.x * currentForce * UnityEngine_Time__get_deltaTime(0);
                    moveVector.fields.y = this->fields.direction.fields.y * currentForce * UnityEngine_Time__get_deltaTime(0);
                    moveVector.fields.z = this->fields.direction.fields.z * currentForce * UnityEngine_Time__get_deltaTime(0);

                    if (!charController)
                        sub_180157B40(0);

                    UnityEngine_CharacterController__Move(charController, &moveVector, 0);
                }
                else
                {
                    this->fields.playerController = 0;
                    sub_180157590(&this->fields.playerController, 0);
                }

                decaySpeed = this->fields.decaySpeed;
                this->fields.currentForce = UnityEngine_Mathf__Lerp(this->fields.currentForce, 0.0, UnityEngine_Time__get_deltaTime(0) * decaySpeed, 0);
            }
        }

		public Bully()
		{
            static bool initialized = false;
            UnityEngine_Vector3_o zeroVec[2];

            this->fields.decaySpeed = 5.0;
            this->fields.direction = *UnityEngine_Vector3__get_zero(zeroVec, 0);
            UnityEngine_Terrain___ctor((UnityEngine_Terrain_o*)this, 0);

            if (!initialized)
            {
                initialized = true;
            }
        }

		[SerializeField]
		private float pushForce;

		[SerializeField]
		private float decaySpeed;

		[SerializeField]
		private Vector3 direction;

		private CharacterController playerController;

		private float currentForce;
	}
}
