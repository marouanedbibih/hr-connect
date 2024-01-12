// apiFunctions.js

import axiosClient from '../api/axios';
import { useStateContext } from '../context/ContextProvider';

const apiFunctions = (sectionName,getFunction) => {
  const { setNotification } = useStateContext();

  const getAllItems = async () => {
    try {
      const response = await axiosClient.get(`/${sectionName}`);
      return response.data;
    } catch (error) {
      console.error(error);
      return [];
    }
  };

  const getItemById = async (id) => {
    try {
      const response = await axiosClient.get(`/${sectionName}/${id}`);
      return response.data;
    } catch (error) {
      console.error(error);
      return null;
    }
  };

  const createItem = async (data) => {
    try {
      await axiosClient.post(`/${sectionName}`, data);
      setNotification(`${sectionName} was successfully created`);
    } catch (error) {
      console.error(error);
      throw error;
    }
  };

  const updateItem = async (id, data) => {
    try {
      await axiosClient.put(`/${sectionName}/${id}`, data);
      setNotification(`${sectionName} was successfully updated`);
    } catch (error) {
      console.error(error);
      throw error;
    }
  };

  const apiDelete = async (id) => {
    if (
        !window.confirm(`Are you sure you want to delete this ${sectionName} !!`)
      ) {
        return;
      }
    try {
      await axiosClient.delete(`/${sectionName}/${id}`);
      setNotification(`${sectionName} was successfully deleted`);
      
    } catch (error) {
      console.error(error);
      throw error;
    }
  };

  return { getAllItems, getItemById, createItem, updateItem, apiDelete };
};

export default apiFunctions;
