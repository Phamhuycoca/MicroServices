// slices/userSlice.ts
import { createSlice, type PayloadAction } from '@reduxjs/toolkit'

interface UserState {
  accessToken: string | null
}

const initialState: UserState = {
  accessToken: null
}

const userSlice = createSlice({
  name: 'user',
  initialState,
  reducers: {
    setAccessToken(state, action: PayloadAction<string>) {
      state.accessToken = action.payload
    }
  }
})

export const { setAccessToken } = userSlice.actions
export default userSlice.reducer
